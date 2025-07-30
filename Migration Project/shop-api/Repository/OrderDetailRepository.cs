using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Models;

namespace shop_api.Repository;

public class OrderDetailRepository : Repository<(int OrderID, int ProductID), OrderDetail>
    {

        public OrderDetailRepository(ShopContext context) : base(context)
        {
        }

        public override async Task<OrderDetail> GetById((int OrderID, int ProductID) id)
        {
            var orderDetail = await _shopContext.OrderDetails
                // .Include(od => od.Order)
                // .Include(od => od.Product)
                .SingleOrDefaultAsync(od => od.OrderID == id.OrderID && od.ProductID == id.ProductID);

            if (orderDetail == null)
                throw new KeyNotFoundException($"OrderDetail with OrderID {id.OrderID} and ProductID {id.ProductID} not found.");

            return orderDetail;
        }

        public override async Task<IEnumerable<OrderDetail>> GetAll()
        {
            return await _shopContext.OrderDetails
                // .Include(od => od.Order)
                // .Include(od => od.Product)
                .ToListAsync();
        }
    }