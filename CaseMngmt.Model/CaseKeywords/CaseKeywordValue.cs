using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.CaseKeywords
{
    public class CaseKeywordValue
    {
        [Required]
        public Guid KeywordId { get; set; }
        [Required]
        public string? Value { get; set; }
        public string? KeywordName { get; set; }
        public bool IsRequired { get; set; }
        public int? MaxLength { get; set; }
        public bool Searchable { get; set; }
        public int Order { get; set; }
        public Guid? TypeId { get; set; }
        public string? TypeName { get; set; }
    }
}
