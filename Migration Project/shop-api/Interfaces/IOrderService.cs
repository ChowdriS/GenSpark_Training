using System;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Interfaces;

public interface IOrderService
{
    public Task<IEnumerable<Order>> GetAll();
    public Task<Order> GetById(int id);
    public Task<Order> Create(OrderRequestDTO dto);
    public Task<Order> Update(int id, OrderUpdateRequestDTO dto);
    public Task<Order> Delete(int id);
    public Task<byte[]> ExportOrdersToPdf();
}
