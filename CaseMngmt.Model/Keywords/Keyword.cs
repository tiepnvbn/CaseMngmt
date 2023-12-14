using CaseMngmt.Models.Cases;
using CaseMngmt.Models.Templates;

namespace CaseMngmt.Models.Keywords
{
    public class Keyword : BaseModel
    {
        public Guid TypeId { get; set; }
        public virtual ICollection<Case> Cases { get; set; }
        public virtual ICollection<Template> Templates { get; set; }
    }
}
