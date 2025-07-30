using System;

namespace shop_api.Models.DTO;

public class PaginatedResult<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<T> Items { get; set; } = new List<T>();
}
