using System;

namespace shop_api.Models;

public class Cart
{
    public Product? Product { get; set; }
    public int Quantity { get; set; }
}
