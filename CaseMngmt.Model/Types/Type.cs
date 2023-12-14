using CaseMngmt.Models.Keywords;
using CaseMngmt.Models.Metadatas;

namespace CaseMngmt.Models.Types
{
    public class Type : BaseModel
    {
        public virtual ICollection<Keyword> Keywords { get; set; }
        public virtual ICollection<Metadata> Metadatas { get; set; }
    }
}
