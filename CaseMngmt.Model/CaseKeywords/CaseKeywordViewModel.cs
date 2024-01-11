using CaseMngmt.Models.Customers;

namespace CaseMngmt.Models.CaseKeywords
{
    public class CaseKeywordViewModel
    {
        public Guid CaseId { get; set; }
        public string CaseName { get; set; }
        public string Status { get; set; }
        public IEnumerable<CaseKeywordBaseValue> CaseKeywordValues { get; set; }

        public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();
    }
}
