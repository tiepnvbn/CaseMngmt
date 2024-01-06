using CaseMngmt.Models;
using CaseMngmt.Models.FileUploads;
using Microsoft.AspNetCore.Http;

namespace CaseMngmt.Service.FileUploads
{
    public interface IFileUploadService
    {
        Task<FileUploadResponse?> UploadFileAsync(IFormFile fileToUpload, Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting);
        Task<int> DeleteFileAsync(string filename, Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting);
        Task<List<string?>> GetAllFileByCaseIdAsync(Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting);
        Task<string?> GetUploadedFolderPath(Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting);
        Task<string?> GetFilePath(string filename, Guid caseId, FileUploadSetting fileSetting, AWSSetting? awsSetting);
    }
}
