using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Models;

namespace shop_api.Repository;

public class NewsRepository : Repository<int, News>
    {

        public NewsRepository(ShopContext context) : base(context)
        {
        }

        public override async Task<News> GetById(int id)
        {
            var news = await _shopContext.News
                // .Include(n => n.User)
                .SingleOrDefaultAsync(n => n.NewsId == id);

            if (news == null)
                throw new KeyNotFoundException($"News with id {id} not found.");

            return news;
        }

        public override async Task<IEnumerable<News>> GetAll()
        {
            return await _shopContext.News
                // .Include(n => n.User)
                .ToListAsync();
        }
    }