using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.KeywordRoles
{
    public class KeywordRole
    {
        [Required]
        [Key]
        public Guid KeywordId { get; set; }

        [Required]
        [Key]
        public Guid RoleId { get; set; }
    }
}
