using CaseMngmt.Models.TemplateKeywords;
using Microsoft.AspNetCore.Identity;

namespace CaseMngmt.Models.ApplicationRoles
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public override Guid Id { get; set; }
        //public virtual ICollection<TemplateKeyword> TemplateKeyword { get; set; }
    }
}
