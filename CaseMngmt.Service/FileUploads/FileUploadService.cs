using AutoMapper;
using CaseMngmt.Models.FileUploads;
using Microsoft.AspNetCore.Http;

namespace CaseMngmt.Service.FileUploads
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IMapper _mapper;
        public FileUploadService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<int> PostFileAsync(IFormFile fileToUpload, FileUploadModel fileDetail)
        {
            try
            {
                var fileSetting = new FileUploadSettings();
                var uploadFolder = fileSetting.UploadFolder + @"\";

                string ext = Path.GetExtension(fileDetail.FileName).ToLower();
                string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
                var fileNameOnServer = Path.Combine(uploadFolder, fileName + ext);

                // Create a stream to write the file to
                using var stream = File.Create(fileNameOnServer);

                // Upload file and copy to the stream
                await fileToUpload.CopyToAsync(stream);

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task DownloadFileById(string fileName)
        {
            try
            {
                var content = new System.IO.MemoryStream();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName);

                await CopyStream(content, path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
    }
}
