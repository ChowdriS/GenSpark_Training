using System;
using EventBookingApi.Context;
using EventBookingApi.Model;
using Microsoft.EntityFrameworkCore;

namespace EventBookingApi.Repository;

public class UserRepository : Repository<string, User>
    {
        public UserRepository(EventContext context):base(context)
        {
            
        }
        public override async Task<User> GetById(string email)
        {
            return await _eventContext?.Users?.SingleOrDefaultAsync(u => u.Email == email);
        }

    public override async Task<IEnumerable<User>> GetAll()
    {
            return await _eventContext.Users.ToListAsync();
    }
            
    }
