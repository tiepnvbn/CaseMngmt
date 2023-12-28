using CaseMngmt.Models.CaseKeywords;
using CaseMngmt.Models.Keywords;

namespace CaseMngmt.Models.Cases
{
    public class Case : BaseModel
    {
        //One Case Can be Taken by Many Keywords
        public List<Keyword> Keywords { get; set; } = new();
        //CaseKeywords Collection Property for Implementing Many to Many Relationship
        public List<CaseKeyword> CaseKeywords { get; set; }
    }
}
