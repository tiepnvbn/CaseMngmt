using CaseMngmt.Models.FileTypes;
using CaseMngmt.Models.Keywords;

namespace CaseMngmt.Models.CaseKeywords
{
    public class DocumentTemplateResponse
    {
        public List<KeywordSearchModel> Keywords { get; set; }
        public FileTypeSearchModel FileType { get; set; }
    }
}
