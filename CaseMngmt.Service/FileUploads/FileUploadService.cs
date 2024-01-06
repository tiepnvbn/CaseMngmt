﻿using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Amazon;
using Amazon.S3;
using Amazon.Runtime;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CaseMngmt.Models;
using CaseMngmt.Models.FileUploads;

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

        public async Task<List<string?>> GetAllFileByCaseIdAsync(Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting)
        {
            try
            {
                var folderPath = await GetUploadedFolderPath(caseId, fileSetting, awsSetting);
                if (folderPath == null)
                {
                    return new List<string?>();
                }

                if (awsSetting == null)
                {
                    return Directory.GetFiles(folderPath, "*.*")
                   .Select(Path.GetFileName)
                   .ToList();
                }
                else
                {
                    BasicAWSCredentials credentials = new BasicAWSCredentials(awsSetting.ACCESS_KEY, awsSetting.SECRET_KEY);
                    using (var client = new AmazonS3Client(credentials, RegionEndpoint.APNortheast1))
                    {
                        ListObjectsV2Request request = new ListObjectsV2Request
                        {
                            BucketName = awsSetting.S3Bucket,
                            Prefix = $"{folderPath}/"
                        };

                        ListObjectsV2Response response = await client.ListObjectsV2Async(request);
                        return response.S3Objects.Select(x => x.Key).ToList();
                    }
                }
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

                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = awsSetting.S3Bucket,
                    Prefix = $"{awsSetting.UploadFolder}/"
                };

                ListObjectsV2Response response = await client.ListObjectsV2Async(request);

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

        public async Task<int> DeleteFileAsync(string filename, Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting)
        {
            try
            {
                var filePath = await GetFilePath(filename, caseId, fileSetting, awsSetting);
                if (filePath == null)
                {
                    return 0;
                }

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
                    BasicAWSCredentials credentials = new BasicAWSCredentials(awsSetting.ACCESS_KEY, awsSetting.SECRET_KEY);
                    using (var client = new AmazonS3Client(credentials, RegionEndpoint.APNortheast1))
                    {
                        var deleteFileRequest = new Amazon.S3.Model.DeleteObjectRequest
                        {
                            BucketName = awsSetting.S3Bucket,
                            Key = filePath
                        };
                        DeleteObjectResponse fileDeleteResponse = await client.DeleteObjectAsync(deleteFileRequest);
                        return fileDeleteResponse.HttpStatusCode == HttpStatusCode.NoContent ? 1 : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        
        public async Task<string?> GetFilePath(string filename, Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting)
        {
            var folderPath = await GetUploadedFolderPath(caseId, fileSetting, awsSetting);

            if (folderPath == null)
            {
                return null;
            }
            string ext = Path.GetExtension(filename).ToLower();
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filename);

            var exactPath = awsSetting == null 
                ? Path.Combine(folderPath, $"{fileNameWithoutExt}{ext}")
                : $"{folderPath}/{fileNameWithoutExt}{ext}";

            return exactPath;
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
                    ContentBody = string.Empty
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
                            StorageClass = S3StorageClass.Standard
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
