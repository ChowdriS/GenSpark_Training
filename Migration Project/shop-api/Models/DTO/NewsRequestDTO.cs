using System;

namespace shop_api.Models.DTO;

public class NewsRequestDTO
{
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? Image { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public int Status { get; set; }
}
