using System;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Interfaces;

public interface IShoppingCartService
{
    Task<List<Cart>> GetCartItems(ISession session);
    Task AddToCart(ISession session, int productId);
    Task UpdateCart(ISession session, Dictionary<int,int> productQuantities);
    Task RemoveFromCart(ISession session, int productId);
    Task ClearCart(ISession session);
    Task<Order> ProcessOrder(ISession session, OrderRequestDTO orderDto);
}
