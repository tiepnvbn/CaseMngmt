﻿using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Server.Models.Account
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
