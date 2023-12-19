using CaseMngmt.Models.KeywordRoles;
using CaseMngmt.Models.Keywords;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMngmt.Models.ApplicationRoles
{
    [Table("AspNetRoles")]
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        //public virtual ICollection<Keyword> Keywords { get; set; }
         public virtual ICollection<KeywordRole> KeywordRole { get;  }
    }
}
