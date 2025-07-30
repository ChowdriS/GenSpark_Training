using System;
using System.Text.Json;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IRepository<int, Product> _productRepository;
    private readonly IRepository<int, Order> _orderRepository;
    private readonly IRepository<(int,int), OrderDetail> _orderDetailRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private const string SessionCartKey = "Cart";

    public ShoppingCartService(
        IRepository<int, Product> productRepository,
        IRepository<int, Order> orderRepository,
        IRepository<(int,int), OrderDetail> orderDetailRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Cart>> GetCartItems(ISession session)
    {
        var cartJson = session.GetString(SessionCartKey);
        if (string.IsNullOrEmpty(cartJson))
        {
            return new List<Cart>();
        }
        return JsonSerializer.Deserialize<List<Cart>>(cartJson)!;
    }

    private void SaveCart(ISession session, List<Cart> carts)
    {
        session.SetString(SessionCartKey, JsonSerializer.Serialize(carts));
    }

    public async Task AddToCart(ISession session, int productId)
    {
        var carts = await GetCartItems(session);
        var index = carts.FindIndex(c => c?.Product?.ProductId == productId);
        if (index == -1)
        {
            var product = await _productRepository.GetById(productId);
            carts.Add(new Cart { Product = product, Quantity = 1 });
        }
        else
        {
            carts[index].Quantity++;
        }
        SaveCart(session, carts);
    }

    public async Task UpdateCart(ISession session, Dictionary<int,int> productQuantities)
    {
        var carts = await GetCartItems(session);
        foreach(var cart in carts)
        {
            if (productQuantities.ContainsKey(cart.Product.ProductId))
            {
                int qty = productQuantities[cart.Product.ProductId];
                // Update the cart item quantity
                cart.Quantity = qty;
            }
        }
        SaveCart(session, carts);
    }


    public async Task RemoveFromCart(ISession session, int productId)
    {
        var carts = await GetCartItems(session);
        var index = carts.FindIndex(c => c?.Product?.ProductId == productId);
        if (index >= 0)
            carts.RemoveAt(index);
        SaveCart(session, carts);
    }
    
    public async Task ClearCart(ISession session)
    {
        session.Remove(SessionCartKey);
    }

    public async Task<Order> ProcessOrder(ISession session, OrderRequestDTO orderDto)
    {
        var carts = await GetCartItems(session);

        if (carts.Count == 0)
            throw new Exception("Cart is empty.");

        var order = new Order
        {
            CustomerName = orderDto.CustomerName,
            CustomerPhone = orderDto.CustomerPhone,
            CustomerEmail = orderDto.CustomerEmail,
            CustomerAddress = orderDto.CustomerAddress, 
            OrderDate = DateTime.Now,
            Status = "Processing"
        };

        await _orderRepository.Add(order); 

        foreach (var cart in carts)
        {
            var orderDetail = new OrderDetail
            {
                OrderID = order.OrderID,
                ProductID = cart.Product.ProductId,
                Quantity = cart.Quantity,
                Price = cart.Product.Price ?? 0
            };
            await _orderDetailRepository.Add(orderDetail);
        }

        await ClearCart(session);
        return order;
    }
}
