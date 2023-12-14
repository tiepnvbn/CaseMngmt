using CaseMngmt.Models.Keywords;

namespace CaseMngmt.Models.Cases
{
    public class Case : BaseModel
    {
        public virtual ICollection<Keyword> Keywords { get; set; }
    }
}
