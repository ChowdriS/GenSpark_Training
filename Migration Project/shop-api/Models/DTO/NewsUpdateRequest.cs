using System;

namespace shop_api.Models.DTO;

public class NewsUpdateRequestDTO
{
    public string? Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? Image { get; set; }
    public string? Content { get; set; }
    public int? Status { get; set; }
}
