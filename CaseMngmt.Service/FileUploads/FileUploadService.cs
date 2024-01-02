using AutoMapper;
using CaseMngmt.Models.FileUploads;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace CaseMngmt.Service.FileUploads
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IMapper _mapper;
        public FileUploadService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> UploadFileAsync(IFormFile fileToUpload, string filePath)
        {
            try
            {
                var currentFilePath = filePath;
                var count = 0;
                while (File.Exists(currentFilePath))
                {
                    count++;
                    currentFilePath = Path.GetDirectoryName(filePath)
                                     + Path.DirectorySeparatorChar
                                     + Path.GetFileNameWithoutExtension(filePath)
                                     + count.ToString()
                                     + Path.GetExtension(filePath);
                }
                // Create a stream to write the file to
                using var stream = File.Create(currentFilePath);

                // Upload file and copy to the stream
                await fileToUpload.CopyToAsync(stream);

                return Path.GetFileNameWithoutExtension(currentFilePath);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string?> GetAllFileByCaseIdAsync(Guid caseId)
        {
            try
            {
                var folderPath = GetUploadedFolderPath(caseId);
                var result = Directory.GetFiles(folderPath, "*.*")
                    .Select(Path.GetFileName)
                    .ToList();
                return result;
            }
            catch (Exception)
            {
                return new List<string?>();
            }
        }

        public string GetUploadedFolderPath(Guid caseId)
        {
            var fileSetting = new FileUploadSettings();
            var uploadFolder = fileSetting.UploadFolder;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), $"{uploadFolder}\\{caseId}");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }

        public int DeleteFileByFilePath(string filePath)
        {
            try
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
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
