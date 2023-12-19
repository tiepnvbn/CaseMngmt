using CaseMngmt.Models.Cases;
using CaseMngmt.Models.ApplicationRoles;
using CaseMngmt.Models.KeywordRoles;

namespace CaseMngmt.Models.Keywords
{
    public class Keyword : BaseModel
    {
        public Guid TypeId { get; set; }

        public Guid TemplateId { get; set; }

        public bool Searchable { get; set; }
        public int Order { get; set; }
        
        public virtual ICollection<Case> Cases { get; set; }
        //public virtual ICollection<ApplicationRole> ApplicationRole { get; set; }
        public virtual ICollection<KeywordRole> KeywordRole { get; set; }
    }
}
