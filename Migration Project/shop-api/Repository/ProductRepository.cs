using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Models;

namespace shop_api.Repository;

public class ProductRepository : Repository<int, Product>
    {

        public ProductRepository(ShopContext context) : base(context)
        {
        }

        public override async Task<Product> GetById(int id)
        {
            var product = await _shopContext.Products
                // .Include(p => p.Category)
                // .Include(p => p.Color)
                // .Include(p => p.Model)
                // .Include(p => p.User)
                // .Include(p => p.OrderDetails)
                .SingleOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                throw new KeyNotFoundException($"Product with id {id} not found.");

            return product;
        }

        public override async Task<IEnumerable<Product>> GetAll()
        {
            return await _shopContext.Products
                // .Include(p => p.Category)
                // .Include(p => p.Color)
                // .Include(p => p.Model)
                // .Include(p => p.User)
                // .Include(p => p.OrderDetails)
                .ToListAsync();
        }
    }