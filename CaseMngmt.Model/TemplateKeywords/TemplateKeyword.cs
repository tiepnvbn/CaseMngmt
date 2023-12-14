using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.CaseKeywords
{
    public class TemplateKeyword
    {
        [Required]
        [Key]
        public Guid TemplateId { get; set; }
        [Required]
        [Key]
        public Guid KeywordId { get; set; }
        public Guid RoleId { get; set; }
        public bool Searchable { get; set; }
        public int Order { get; set; }
    }
}
