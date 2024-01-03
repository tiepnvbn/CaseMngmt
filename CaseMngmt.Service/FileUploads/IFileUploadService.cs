using Microsoft.AspNetCore.Http;

namespace CaseMngmt.Service.FileUploads
{
    public interface IFileUploadService
    {
        /// <summary>
        /// Upload file and return the FileName after uploaded
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<string> UploadFileAsync(IFormFile fileToUpload, string filePath);
        int DeleteFileByFilePath(string filePath);
        List<string?> GetAllFileByCaseIdAsync(Guid caseId);
        string GetUploadedFolderPath(Guid caseId);
    }
}
