using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Models;

namespace shop_api.Repository;

public class ModelRepository : Repository<int, Model>
    {

        public ModelRepository(ShopContext context) : base(context)
        {
        }

        public override async Task<Model> GetById(int id)
        {
            var model = await _shopContext.Models
                // .Include(m => m.Products)
                .SingleOrDefaultAsync(m => m.ModelId == id);

            if (model == null)
                throw new KeyNotFoundException($"Model with id {id} not found.");

            return model;
        }

        public override async Task<IEnumerable<Model>> GetAll()
        {
            return await _shopContext.Models
                // .Include(m => m.Products)
                .ToListAsync();
        }
    }