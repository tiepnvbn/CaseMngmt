using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.CaseKeywords
{
    public class KeywordValue
    {
        [Required]
        public Guid KeywordId { get; set; }
        [Required]
        public string? Value { get; set; }
    }
}
