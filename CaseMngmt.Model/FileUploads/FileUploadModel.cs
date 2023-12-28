using Microsoft.AspNetCore.Http;

namespace CaseMngmt.Models.FileUploads
{
    public class FileUploadModel
    {
        public string FileName { get; set; }
        public IFormFile FileToUpload { get; set; }
    }
}
