using CaseMngmt.Models.FileUploads;
using Microsoft.AspNetCore.Http;

namespace CaseMngmt.Service.FileUploads
{
    public interface IFileUploadService
    {
        Task<int> PostFileAsync(IFormFile fileToUpload, FileUploadModel fileDetail);
        Task DownloadFileById(string fileName);
    }
}
