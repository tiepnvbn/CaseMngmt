using CaseMngmt.Models.Keywords;

namespace CaseMngmt.Models.Types
{
    public class Type : BaseModel
    {
        public string Value { get; set; }
        public virtual ICollection<Keyword> Keywords { get; set; }
    }
}
