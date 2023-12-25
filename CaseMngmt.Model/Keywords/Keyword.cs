using CaseMngmt.Models.Cases;
using CaseMngmt.Models.KeywordRoles;
using Type = CaseMngmt.Models.Types.Type;

namespace CaseMngmt.Models.Keywords
{
    public class Keyword : BaseModel
    {
        public Guid TypeId { get; set; }
        public Guid TemplateId { get; set; }
        public int? MaxLength { get; set; }
        public bool IsRequired { get; set; }
        public bool Searchable { get; set; }
        public int Order { get; set; }
        // TODO Source
        public string? Source { get; set; }
        public string? Metadata { get; set; }
        public Type Type { get; set; }
        public virtual ICollection<Case> Cases { get; set; }
        public virtual ICollection<KeywordRole> KeywordRole { get; set; }
    }
}
