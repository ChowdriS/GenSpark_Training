using System;
using Microsoft.EntityFrameworkCore;
using shop_api.Context;
using shop_api.Models;

namespace shop_api.Repository;

public class ContactURepository : Repository<int, ContactU>
    {

        public ContactURepository(ShopContext context) : base(context)
        {
           
        }

        public override async Task<ContactU> GetById(int id)
        {
            var contact = await _shopContext.ContactUs
                .SingleOrDefaultAsync(c => c.id == id);

            if (contact == null)
                throw new KeyNotFoundException($"Contact with id {id} not found.");

            return contact;
        }

        public override async Task<IEnumerable<ContactU>> GetAll()
        {
            return await _shopContext.ContactUs
                .ToListAsync();
        }
    }