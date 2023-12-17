using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.TemplateKeywords
{
    public class TemplateKeywordViewModel
    {
        [Required]
        public Guid TemplateId { get; set; }
        [Required]
        public List<TemplateKeywordValue> TemplateKeywordValues { get; set; }
    }
}
