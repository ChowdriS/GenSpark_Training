using System;
using BankingAppApi.Context;
using BankingAppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingAppApi.Repositoy;

public class CustomerRepository : Repository<int, Customer>
{
    public CustomerRepository(BankContext bankContext) : base(bankContext)
    {
    }

    public override async Task<Customer> GetById(int key)
    {
        var customer = await _bankContext.Customers
            .Include(c => c.Accounts)
            .SingleOrDefaultAsync(p => p.Id == key);

        return customer ?? throw new Exception("No Customer with the given ID");
    }

    public override async Task<IEnumerable<Customer>> GetAll()
    {
        var Customers = _bankContext.Customers.Include(c => c.Accounts);
        if (Customers.Count() == 0)
            throw new Exception("No Customers in the database");
        return await Customers.ToListAsync();
    }
}
