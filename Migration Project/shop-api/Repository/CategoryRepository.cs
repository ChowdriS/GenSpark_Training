using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Models;

namespace shop_api.Repository;

public class CategoryRepository : Repository<int, Category>
    {

        public CategoryRepository(ShopContext context) : base(context)
        {
        }

        public override async Task<Category> GetById(int id)
        {
            var category = await _shopContext.Categories
                // .Include(c => c.Products)
                .SingleOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                throw new KeyNotFoundException($"Category with id {id} not found.");

            return category;
        }

        public override async Task<IEnumerable<Category>> GetAll()
        {
            return await _shopContext.Categories
                // .Include(c => c.Products)
                .ToListAsync();
        }
    }