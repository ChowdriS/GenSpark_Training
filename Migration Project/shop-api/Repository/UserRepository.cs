using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Models;

namespace shop_api.Repository;

public class UserRepository : Repository<int, User>
{
    private readonly ShopContext _context;

    public UserRepository(ShopContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<User> GetById(int id)
    {
        var user = await _shopContext.Users
            // .Include(u => u.News)       
            // .Include(u => u.Products)
            .SingleOrDefaultAsync(u => u.UserId == id);

        if (user == null)
            throw new KeyNotFoundException($"User with id {id} not found.");

        return user;
    }

    public override async Task<IEnumerable<User>> GetAll()
    {
        return await _shopContext.Users
            // .Include(u => u.News)
            // .Include(u => u.Products)
            .ToListAsync();
    }
}