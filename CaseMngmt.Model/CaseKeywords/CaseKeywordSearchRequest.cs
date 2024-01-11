
using System.Globalization;

namespace CaseMngmt.Models.CaseKeywords
{
    public class CaseKeywordSearch
    {
        public List<KeywordValue> KeywordValues { get; set; }
        public List<KeywordSearchRangeValue> KeywordDateValues { get; set; }
        public int PageSize { get; set; } = 25;
        public int PageNumber { get; set; } = 1;

        public bool IsValidDatetime()
        {
            try
            {
                if (KeywordDateValues != null && KeywordDateValues.Any())
                {
                    foreach (var item in KeywordDateValues)
                    {
                        if (!string.IsNullOrEmpty(item.FromValue) && string.IsNullOrEmpty(item.ToValue))
                        {
                            item.FromValue.Replace("-", "/");
                            DateTime.TryParse(item.FromValue, out _);
                            item.FromValue = DateTime.ParseExact(item.FromValue, "yyyy/MM/dd", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                        }
                        else if (string.IsNullOrEmpty(item.FromValue) && !string.IsNullOrEmpty(item.ToValue))
                        {
                            item.ToValue.Replace("-", "/");
                            DateTime.TryParse(item.ToValue, out _);
                            item.ToValue = DateTime.ParseExact(item.ToValue, "yyyy/MM/dd", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                        }
                        else
                        {
                            item.FromValue.Replace("-", "/");
                            item.ToValue.Replace("-", "/");
                            DateTime.TryParse(item.FromValue, out _);
                            DateTime.TryParse(item.ToValue, out _);
                            item.FromValue = DateTime.ParseExact(item.FromValue, "yyyy/MM/dd", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                            item.ToValue = DateTime.ParseExact(item.ToValue, "yyyy/MM/dd", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class CaseKeywordSearchRequest : CaseKeywordSearch
    {
        public List<Guid> RoleIds { get; set; }
        public Guid TemplateId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
