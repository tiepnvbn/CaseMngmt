using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace CaseMngmt.Service.FileUploads
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IMapper _mapper;
        public FileUploadService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<int> UploadFileAsync(IFormFile fileToUpload, string filePath)
        {
            try
            {
                // Create a stream to write the file to
                using var stream = File.Create(filePath);

                // Upload file and copy to the stream
                await fileToUpload.CopyToAsync(stream);

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task DownloadFileByFileName(string filePath)
        {
            try
            {
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var bytes = await File.ReadAllBytesAsync(filePath);
                //var result = new File(bytes, contentType, Path.GetFileName(path));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
