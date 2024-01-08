namespace CaseMngmt.Models.FileUploads
{
    public class DeleteFileRequest
    {
        public Guid KeywordId { get; set; }
        public Guid CaseId { get; set; }
        public string Filename { get; set; }
    }

    public class DownloadFileRequest
    {
        public Guid CaseId { get; set; }
        public string Filename { get; set; }
    }
}
