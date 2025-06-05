using System;
using AppointmentApi.Context;
using AppointmentApi.Models;

using Microsoft.EntityFrameworkCore;
namespace AppointmentApi.Repository;

public class UserRepository : Repository<string, User>
    {
        public UserRepository(ClinicContext context):base(context)
        {
            
        }
        public override async Task<User> GetById(string key)
        {
            // throw new NotImplementedException();
            return await _clinicContext.Users.SingleOrDefaultAsync(u => u.Username == key);
        }

    public override async Task<IEnumerable<User>> GetAll()
    {
            // throw new NotImplementedException();
            return await _clinicContext.Users.ToListAsync();
    }
            
    }
