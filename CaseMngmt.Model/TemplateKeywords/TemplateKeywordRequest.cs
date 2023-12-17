using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.TemplateKeywords
{
    public class TemplateKeywordRequest
    {
        [Required]
        public Guid TemplateId { get; set; }
        [Required]
        public List<TemplateKeywordValue> KeywordValues { get; set; }
    }
}
