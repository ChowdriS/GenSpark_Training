using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_api.Models;

public class News
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NewsId { get; set; }
    public int? UserId { get; set; }
    public string? Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? Image { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? Status { get; set; }

    public User? User { get; set; }
}
