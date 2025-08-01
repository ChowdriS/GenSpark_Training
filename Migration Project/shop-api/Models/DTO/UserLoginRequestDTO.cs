using System;
using System.ComponentModel.DataAnnotations;

namespace shop_api.Models.DTO;

public class UserLoginRequestDTO
    {
        [Required(ErrorMessage = "Username is manditory")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is manditory")]
        public string Password { get; set; } = string.Empty;
    }
