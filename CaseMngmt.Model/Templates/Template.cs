using CaseMngmt.Models.CompanyTemplates;
using CaseMngmt.Models.Keywords;

namespace CaseMngmt.Models.Templates
{
    public class Template : BaseModel
    {
        //public Template(Guid updatedUserId)
        //{
        //    CreatedBy = updatedUserId;
        //    UpdatedBy = updatedUserId;
        //}
        public virtual ICollection<Keyword> Keywords { get; set; }
        public virtual ICollection<CompanyTemplate> CompanyTemplate { get; set; }
    }
}
