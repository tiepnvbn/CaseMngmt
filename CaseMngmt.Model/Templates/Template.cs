using CaseMngmt.Models.ApplicationUsers;
using CaseMngmt.Models.Keywords;

namespace CaseMngmt.Models.Templates
{
    public class Template : BaseModel
    {
        public virtual ICollection<Keyword> Keywords { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
