namespace CaseMngmt.Models.CaseKeywords
{
    public class CaseKeywordViewModel
    {
        public Guid CaseId { get; set; }
        public string CaseName { get; set; }
        public IEnumerable<CaseKeywordBaseValue> CaseKeywordValues { get; set; }
    }
}
