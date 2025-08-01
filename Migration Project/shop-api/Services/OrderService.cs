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

        // Start full HTML document with minimal styling for Excel
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
        sb.AppendLine("<style>");
        sb.AppendLine("table { border-collapse: collapse; }");
        sb.AppendLine("th, td { border: 1px solid black; padding: 5px; text-align: left; }");
        sb.AppendLine("th { background-color: #F2B134; }"); // orange-yellow header to match your app theme
        sb.AppendLine("</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        
        sb.AppendLine("<h2>Order Report</h2>");
        sb.AppendLine("<table>");
        sb.AppendLine("<thead>");
        sb.AppendLine("<tr><th>Order ID</th><th>Order Name</th><th>Order Date</th></tr>");
        sb.AppendLine("</thead>");
        sb.AppendLine("<tbody>");

        foreach (var order in orders)
        {
            // Format date as yyyy-MM-dd or other preferred format
            var formattedDate = order.OrderDate.ToString("yyyy-MM-dd");
            // Simple HTML encode (replace &, <, >), expand if needed
            var orderNameEscaped = System.Net.WebUtility.HtmlEncode(order.OrderName ?? "");
            
            sb.AppendLine($"<tr><td>{order.OrderID}</td><td>{orderNameEscaped}</td><td>{formattedDate}</td></tr>");
        }

        sb.AppendLine("</tbody>");
        sb.AppendLine("</table>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

}
