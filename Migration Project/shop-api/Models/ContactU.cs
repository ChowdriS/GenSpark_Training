using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_api.Models;

public  class ContactU
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public string? name { get; set; }
    public string? email { get; set; }
    public string? phone { get; set; }
    public string? content { get; set; }
}