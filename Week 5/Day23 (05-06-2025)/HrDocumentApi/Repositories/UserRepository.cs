using HrDocumentApi.Repositories;
using HrDocumentApi.Contexts;
using HrDocumentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HrDocumentApi.Repositories;

public class UserRepository : Repository<string, User>
{
    public UserRepository(HrDocumentApiContext HrDocumentApiContext) : base(HrDocumentApiContext)
    {
    }
    public override async Task<User> Get(string username)
    {
        var user = await _HrDocumentApiContext.Users.Include(u => u.UploadedFiles).FirstOrDefaultAsync(u => u.UserName == username);
        return user ?? throw new KeyNotFoundException($"User with username {username} not found.");
    }

    public override async Task<IEnumerable<User>> GetAll()
    {
        var users = await _HrDocumentApiContext.Users.Include(u => u.UploadedFiles).ToListAsync();
        return users ?? new List<User>();
    }
}