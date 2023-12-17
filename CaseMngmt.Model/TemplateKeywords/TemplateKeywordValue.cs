using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.TemplateKeywords
{
    public class TemplateKeywordValue
    {
        [Required]
        public Guid KeywordId { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        [Required]
        public int Order { get; set; }
        public bool? Searchable { get; set; } = false;
        public Guid? TypeId { get; set; }
        public string? TypeName { get; set; }
    }
}
