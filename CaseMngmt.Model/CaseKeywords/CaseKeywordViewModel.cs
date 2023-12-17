using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.CaseKeywords
{
    public class CaseKeywordViewModel
    {
        [Required]
        public Guid CaseId { get; set; }
        [Required]
        public List<CaseKeywordValue> CaseKeywordValues { get; set; }
    }
}
