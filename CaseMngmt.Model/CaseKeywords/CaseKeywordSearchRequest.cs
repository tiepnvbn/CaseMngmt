using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.CaseKeywords
{
    public class CaseKeywordSearchRequest
    {
        [Required]
        public List<KeywordValue> KeywordValues { get; set; }
        [Required]
        public Guid TemplateId { get; set; }
        public Guid? CompanyId { get; set; }
        public int? PageSize { get; set; } = 25;
        public int? PageNumber { get; set; } = 1;
    }
}
