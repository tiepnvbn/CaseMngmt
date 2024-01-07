namespace CaseMngmt.Models.FileUploads
{
    public class FileResponse
    {
        public Guid KeywordId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public bool IsImage { get; set; } = false;
    }

    public class FileUploadResponse
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}
