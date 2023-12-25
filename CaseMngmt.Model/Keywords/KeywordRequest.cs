using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.Keywords
{
    public class KeywordRequest
    {
        [MaxLength(256)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string TypeName { get; set; }
        public int MaxLength { get; set; }
        public bool IsRequired { get; set; }
        public bool Searchable { get; set; }
        public int Order { get; set; }
        public string? Metadata { get; set; }
        public string? Source { get; set; }
    }

    public class KeywordEditRequest : KeywordRequest
    {
        [Required]
        public Guid TemplateId { get; set; }
    }
    public class KeywordAddRequest : KeywordRequest
    {
    }
}
