using Microsoft.AspNetCore.Http;

namespace CaseMngmt.Service.FileUploads
{
    public interface IFileUploadService
    {
        Task<int> UploadFileAsync(IFormFile fileToUpload, string filePath);
        Task DownloadFileByFileName(string filePath);
    }
}
