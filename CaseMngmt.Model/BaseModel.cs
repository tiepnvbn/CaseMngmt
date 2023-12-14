﻿using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models
{
    public class BaseModel
    {
        [Required]
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(256)]
        public string? Name { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
