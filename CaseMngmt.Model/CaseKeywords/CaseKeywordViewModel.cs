namespace CaseMngmt.Models.CaseKeywords
{
    public class CaseKeywordViewModel
    {
        public Guid CaseId { get; set; }
        public List<CaseKeywordBaseValue> CaseKeywordValues { get; set; }
    }
}
