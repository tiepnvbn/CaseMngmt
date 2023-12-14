using CaseMngmt.Models.CaseKeywords;
using Microsoft.AspNetCore.Identity;

namespace CaseMngmt.Models.ApplicationRoles
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public virtual ICollection<TemplateKeyword> TemplateKeyword { get; set; }
    }
}
