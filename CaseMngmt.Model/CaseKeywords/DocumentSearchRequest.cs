using System.Globalization;

namespace CaseMngmt.Models.CaseKeywords
{
    public class DocumentSearch
    {
        public Guid? FileTypeId { get; set; }
        public List<KeywordValue> KeywordValues { get; set; }
        public List<KeywordSearchRangeValue> KeywordDateValues { get; set; }
        public List<KeywordSearchRangeValue> KeywordDecimalValues { get; set; }
        public int PageSize { get; set; } = 25;
        public int PageNumber { get; set; } = 1;

        public bool IsValid()
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

                if (KeywordDecimalValues != null && KeywordDecimalValues.Any())
                {
                    foreach (var item in KeywordDecimalValues)
                    {
                        if (!string.IsNullOrEmpty(item.FromValue) && string.IsNullOrEmpty(item.ToValue))
                        {
                            decimal.TryParse(item.FromValue, out _);
                        }
                        else if (string.IsNullOrEmpty(item.FromValue) && !string.IsNullOrEmpty(item.ToValue))
                        {
                            decimal.TryParse(item.ToValue, out _);
                        }
                        else
                        {
                            decimal.TryParse(item.FromValue, out _);
                            decimal.TryParse(item.ToValue, out _);
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

    public class DocumentSearchRequest : DocumentSearch
    {
        public Guid TemplateId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
