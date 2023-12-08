using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Server.Models
{
    public class BaseModel
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(256)]
        public string? Name { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
