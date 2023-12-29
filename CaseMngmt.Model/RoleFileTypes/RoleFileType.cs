using CaseMngmt.Models.ApplicationRoles;
using CaseMngmt.Models.FileTypes;
using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.RoleFileTypes
{
    public class RoleFileType
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Guid FileTypeId { get; set; }

        public ApplicationRole ApplicationRole { get; set; } = null!;
        public FileType FileType { get; set; } = null!;
    }
}
