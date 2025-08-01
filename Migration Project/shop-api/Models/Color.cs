using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_api.Models;

public class Color
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ColorId { get; set; }
    public string? Color1 { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
}
