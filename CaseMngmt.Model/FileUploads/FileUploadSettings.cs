namespace CaseMngmt.Models.FileUploads
{
    public class FileUploadSettings
    {
        public FileUploadSettings()
        {
            AcceptTypes = "image/*,.pdf,.txt,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.dwg,.dxf,.jww";
            ValidFileTypes = "application/vnd.openxmlformats-officedocument,image/,application/pdf,text/plain";
            InvalidFileExtensions = ".cs,.js,.vb,.exe,.com,.bat";
            UploadFolder = "UploadedFiles";
            UploadFolderPath = string.Empty;
            ThumbScale = 0.15M;
        }

        public string AcceptTypes { get; set; }
        public string ValidFileTypes { get; set; }
        public string InvalidFileExtensions { get; set; }
        public string UploadFolder { get; set; }
        public string UploadFolderPath { get; set; }
        public decimal ThumbScale { get; set; }
    }
}
