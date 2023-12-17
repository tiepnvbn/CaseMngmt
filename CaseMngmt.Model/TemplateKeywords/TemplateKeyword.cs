using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseMngmt.Models.TemplateKeywords
{
    public class TemplateKeyword
    {
        [Required]
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey("TemplateId")]
        public Guid TemplateId { get; set; }

        [Required]
        [ForeignKey("KeywordId")]
        public Guid KeywordId { get; set; }

        [Required]
        [ForeignKey("RoleId")]
        public Guid RoleId { get; set; }
        public bool Searchable { get; set; }
        public int Order { get; set; }
        public bool Deleted { get; set; }
    }
}
