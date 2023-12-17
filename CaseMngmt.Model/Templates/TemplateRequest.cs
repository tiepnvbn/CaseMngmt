using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.Templates
{
    public class TemplateRequest
    {
        [MaxLength(256)]
        [Required]
        public string? Name { get; set; }

        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
