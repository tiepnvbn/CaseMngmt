using CaseMngmt.Models.Keywords;

namespace CaseMngmt.Models.Types
{
    public class Type : BaseModel
    {
        public bool IsDefaultType { get; set; }
        public string Value { get; set; }
        public string? Source { get; set; }
        public string? Metadata { get; set; }
        public List<Keyword> Keywords { get; set; }
    }
}
