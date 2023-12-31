using Microsoft.AspNetCore.Http;

namespace CaseMngmt.Service.FileUploads
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile fileToUpload, string filePath);
        List<string?> GetAllFileByCaseIdAsync(Guid caseId);
        string GetUploadedFolderPath(Guid caseId);
    }
}
