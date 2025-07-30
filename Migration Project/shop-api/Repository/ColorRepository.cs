using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Models;

namespace shop_api.Repository;

public class ColorRepository : Repository<int, Color>
    {

        public ColorRepository(ShopContext context) : base(context)
        {
        }

        public override async Task<Color> GetById(int id)
        {
            var color = await _shopContext.Colors
                // .Include(c => c.Products)
                .SingleOrDefaultAsync(c => c.ColorId == id);

            if (color == null)
                throw new KeyNotFoundException($"Color with id {id} not found.");

            return color;
        }

        public override async Task<IEnumerable<Color>> GetAll()
        {
            return await _shopContext.Colors
                // .Include(c => c.Products)
                .ToListAsync();
        }
    }