using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CaseMngmt.Models;
using CaseMngmt.Models.FileUploads;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.Net;

namespace CaseMngmt.Service.FileUploads
{
    public class FileUploadService : IFileUploadService
    {
        public FileUploadService()
        {
        }

        #region Public methods

        public async Task<FileUploadResponse?> UploadFileAsync(IFormFile fileToUpload, Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting)
        {
            try
            {
                var folderPath = await GetUploadedFolderPath(caseId, fileSetting, awsSetting);
                if (folderPath == null)
                {
                    return null;
                }

                if (awsSetting == null)
                {
                    return await UploadLocalFileAsync(fileToUpload, folderPath);
                }
                else
                {
                    return await UploadAWSS3FileAsync(fileToUpload, folderPath, awsSetting);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string?> GetFilePath(string filename, Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting)
        {
            string ext = Path.GetExtension(filename).ToLower();
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filename);
            var folderPath = await GetUploadedFolderPath(caseId, fileSetting, awsSetting);

            if (folderPath == null)
            {
                return null;
            }

            var exactPath = Path.Combine(folderPath, $"{fileNameWithoutExt}{ext}");
            return exactPath;
        }

        public async Task<List<string?>> GetAllFileByCaseIdAsync(Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting)
        {
            try
            {
                var folderPath = await GetUploadedFolderPath(caseId, fileSetting, awsSetting);
                var result = new List<string?>();

                if (awsSetting == null)
                {
                    result = Directory.GetFiles(folderPath, "*.*")
                   .Select(Path.GetFileName)
                   .ToList();
                }
                else
                {
                    BasicAWSCredentials credentials = new BasicAWSCredentials(awsSetting.ACCESS_KEY, awsSetting.SECRET_KEY);
                    using (var client = new AmazonS3Client(credentials, RegionEndpoint.APNortheast1))
                    {
                        ListObjectsRequest request = new ListObjectsRequest
                        {
                            BucketName = awsSetting.S3Bucket,
                            Prefix = folderPath // "my-folder/sub-folder/"
                        };

                        ListObjectsResponse response = await client.ListObjectsAsync(request);
                        //foreach (Amazon.S3.Model.S3Object obj in response.S3Objects)
                        //{
                        //    Console.WriteLine(obj.Key);
                        //}
                        result = response.S3Objects.Select(x => x.Key).ToList();
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return new List<string?>();
            }
        }

        public async Task<string?> GetUploadedFolderPath(Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting)
        {
            var folderPath = string.Empty;
            if (awsSetting == null)
            {
                var uploadFolder = fileSetting.UploadFolder;

                folderPath = Path.Combine(Directory.GetCurrentDirectory(), $"{uploadFolder}/{caseId}");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
            else
            {
                BasicAWSCredentials credentials = new BasicAWSCredentials(awsSetting.ACCESS_KEY, awsSetting.SECRET_KEY);
                var client = new AmazonS3Client(credentials, RegionEndpoint.APNortheast1);

                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = awsSetting.S3Bucket,
                    Prefix = $"{awsSetting.UploadFolder}/"
                };

                ListObjectsResponse response = await client.ListObjectsAsync(request);

                var existFolder = response.S3Objects.FirstOrDefault(x => x.Key == $"{awsSetting.UploadFolder}/{caseId}/");
                if (existFolder != null)
                {
                    folderPath = $"{awsSetting.UploadFolder}/{caseId}";
                }
                else
                {
                    var awsFolder = await CreateFolder(awsSetting.S3Bucket, $"{awsSetting.UploadFolder}/{caseId}/", awsSetting);
                    folderPath = awsFolder == null ? null : $"{awsSetting.UploadFolder}/{caseId}";
                }
            }

            return folderPath;
        }

        public async Task<int> DeleteFileByFilePath(string filePath, FileUploadSetting fileSetting, AWSSetting? awsSetting)
        {
            try
            {
                if (awsSetting == null)
                {
                    IFileProvider physicalFileProvider = new PhysicalFileProvider(filePath);

                    if (physicalFileProvider is PhysicalFileProvider)
                    {
                        var directory = physicalFileProvider.GetDirectoryContents(string.Empty);
                        foreach (var file in directory)
                        {
                            if (!file.IsDirectory)
                            {
                                var fileInfo = new FileInfo(file.PhysicalPath);
                                fileInfo.Delete();
                            }
                        }
                    }

                    return 1;
                }
                else
                {
                    // TODO
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        #endregion

        #region Private methods

        private async Task<string?> CreateFolder(string awsBucketName, string awsFolderName, AWSSetting awsSetting)
        {
            BasicAWSCredentials credentials = new BasicAWSCredentials(awsSetting.ACCESS_KEY, awsSetting.SECRET_KEY);
            using (var client = new AmazonS3Client(credentials, RegionEndpoint.APNortheast1))
            {
                Amazon.S3.Model.PutObjectRequest putObjectRequest = new Amazon.S3.Model.PutObjectRequest
                {
                    BucketName = awsBucketName,
                    StorageClass = S3StorageClass.Standard,
                    ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256,
                    CannedACL = S3CannedACL.Private,
                    Key = awsFolderName,
                    ContentBody = awsFolderName
                };

                var response = await client.PutObjectAsync(putObjectRequest);

                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    return awsFolderName;
                }
                return null;
            }
        }

        private async Task<bool> CheckFileS3IsExists(string fileName, AmazonS3Client s3Client, AWSSetting awsSetting)
        {
            GetObjectMetadataRequest request = new GetObjectMetadataRequest()
            {
                BucketName = awsSetting.S3Bucket,
                Key = fileName
            };

            try
            {
                var result = await s3Client.GetObjectMetadataAsync(request);
                return result != null && result.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex)
            {
                return false;
            }
        }

        private async Task<FileUploadResponse?> UploadLocalFileAsync(IFormFile fileToUpload, string folderPath)
        {
            var count = 0;
            var currentFilePath = $"{folderPath}/{fileToUpload.FileName}";
            while (File.Exists(currentFilePath))
            {
                count++;
                currentFilePath = Path.GetDirectoryName(currentFilePath)
                                 + Path.DirectorySeparatorChar
                                 + Path.GetFileNameWithoutExtension(currentFilePath)
                                 + count.ToString()
                                 + Path.GetExtension(currentFilePath);
            }
            using var stream = File.Create(currentFilePath);

            await fileToUpload.CopyToAsync(stream);

            return new FileUploadResponse
            {
                FileName = Path.GetFileName(currentFilePath),
                FilePath = currentFilePath
            };
        }

        private async Task<FileUploadResponse?> UploadAWSS3FileAsync(IFormFile fileToUpload, string folderPath, AWSSetting awsSetting)
        {
            try
            {
                BasicAWSCredentials credentials = new BasicAWSCredentials(awsSetting.ACCESS_KEY, awsSetting.SECRET_KEY);
                using (var client = new AmazonS3Client(credentials, RegionEndpoint.APNortheast1))
                {
                    var count = 0;
                    var currentFilePath = $"{folderPath}/{fileToUpload.FileName}";
                    while (await CheckFileS3IsExists(currentFilePath, client, awsSetting))
                    {
                        count++;
                        currentFilePath = folderPath + "/"
                            + Path.GetFileNameWithoutExtension(fileToUpload.FileName)
                            + count.ToString()
                            + Path.GetExtension(fileToUpload.FileName);
                    }

                    using (var newMemoryStream = new MemoryStream())
                    {
                        fileToUpload.CopyTo(newMemoryStream);

                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = newMemoryStream,
                            Key = currentFilePath,
                            BucketName = awsSetting.S3Bucket,
                            CannedACL = S3CannedACL.PublicRead
                        };

                        var fileTransferUtility = new TransferUtility(client);
                        await fileTransferUtility.UploadAsync(uploadRequest);

                        return new FileUploadResponse
                        {
                            FileName = Path.GetFileName(currentFilePath),
                            FilePath = currentFilePath
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
