using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_api.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public UserRole Role { get; set; }

    public ICollection<News>? News { get; set; }
    public ICollection<Product>? Products { get; set; }
}

public enum UserRole
{
    Admin,
    User
}
