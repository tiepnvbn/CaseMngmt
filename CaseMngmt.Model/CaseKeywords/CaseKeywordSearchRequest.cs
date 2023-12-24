
namespace CaseMngmt.Models.CaseKeywords
{
    public class CaseKeywordSearch
    {
        public List<KeywordValue> KeywordValues { get; set; }
        public int PageSize { get; set; } = 25;
        public int PageNumber { get; set; } = 1;
    }

    public class CaseKeywordSearchRequest : CaseKeywordSearch
    {
        public Guid? TemplateId { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
