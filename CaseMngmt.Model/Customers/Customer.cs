using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.Customers
{
    public class Customer : BaseModel
    {
        [MaxLength(12)]
        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? PostCode { get; set; }

        [MaxLength(256)]
        [Required]
        public string? StateProvince { get; set; }

        [MaxLength(256)]
        [Required]
        public string? City { get; set; }

        [MaxLength(256)]
        [Required]
        public string? Street { get; set; }

        [MaxLength(256)]
        [Required]
        public string? BuildingName { get; set; }

        public string? RoomNuber { get; set; }

        [MaxLength(3000)]
        public string? Note { get; set; }
    }
}
