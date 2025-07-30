using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<int, Order> _orderRepository;
    private readonly ShopContext _context; 

    public OrderService(IRepository<int, Order> orderRepository, ShopContext context)
    {
        _orderRepository = orderRepository;
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAll()
    {
        return await _orderRepository.GetAll();
    }

    public async Task<Order> GetById(int id)
    {
        var order = await _orderRepository.GetById(id);
        return order;
    }

    public async Task<Order> Create(OrderRequestDTO dto)
    {
        if (dto == null)
            throw new Exception("OrderRequestDTO cannot be null.");

        if (dto.OrderName == null || dto.OrderName.Trim() == "")
            throw new Exception("Order name is required.");

        if (dto.CustomerName == null || dto.CustomerName.Trim() == "")
            throw new Exception("Customer name is required.");

        var order = new Order
        {
            OrderName = dto.OrderName.Trim(),
            OrderDate = (DateTime)dto.OrderDate!,
            PaymentType = dto.PaymentType?.Trim(),
            Status = dto.Status?.Trim(),
            CustomerName = dto.CustomerName.Trim(),
            CustomerPhone = dto.CustomerPhone?.Trim(),
            CustomerEmail = dto.CustomerEmail?.Trim(),
            CustomerAddress = dto.CustomerAddress?.Trim()
        };

        await _orderRepository.Add(order);
        return order;
    }

    public async Task<Order> Update(int id, OrderUpdateRequestDTO dto)
    {
        if (dto == null)
            throw new Exception("OrderUpdateRequestDTO cannot be null.");

        var order = await _orderRepository.GetById(id);
        
        if (dto.Status != null)
        {
            var trimmed = dto.Status.Trim();
            if (trimmed == "")
                throw new Exception("Status cannot be empty.");
            order.Status = trimmed;
        }
        if (dto.CustomerName != null)
        {
            var trimmed = dto.CustomerName.Trim();
            if (trimmed == "")
                throw new Exception("Customer name cannot be empty.");
            order.CustomerName = trimmed;
        }
        if (dto.CustomerPhone != null)
        {
            order.CustomerPhone = dto.CustomerPhone.Trim();
        }
        if (dto.CustomerEmail != null)
        {
            order.CustomerEmail = dto.CustomerEmail.Trim();
        }
        if (dto.CustomerAddress != null)
        {
            order.CustomerAddress = dto.CustomerAddress.Trim();
        }

        await _orderRepository.Update(id, order);
        return order;
    }

    public async Task<Order> Delete(int id)
    {
        var order = await _orderRepository.GetById(id);
        await _orderRepository.Delete(id);
        return order;
    }

    public async Task<byte[]> ExportOrdersToPdf()
    {
        var orders = await _context.Orders.ToListAsync();
        var sb = new StringBuilder();
        sb.AppendLine("Order Report");
        foreach (var order in orders)
        {
            sb.AppendLine($"{order.OrderID} - {order.OrderName} - {order.OrderDate}");
        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}
