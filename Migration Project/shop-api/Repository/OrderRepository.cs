using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Models;

namespace shop_api.Repository;

public class OrderRepository : Repository<int, Order>
    {

        public OrderRepository(ShopContext context) : base(context)
        {
        }

        public override async Task<Order> GetById(int id)
        {
            var order = await _shopContext.Orders
                // .Include(o => o.OrderDetails)
                .SingleOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
                throw new KeyNotFoundException($"Order with id {id} not found.");

            return order;
        }

        public override async Task<IEnumerable<Order>> GetAll()
        {
            return await _shopContext.Orders
                // .Include(o => o.OrderDetails)
                .ToListAsync();
        }
    }
