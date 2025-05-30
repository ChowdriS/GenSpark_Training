using System;
using BankingAppApi.Context;
using BankingAppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingAppApi.Repositoy;

public class AccountRepository : Repository<long, Account>
{
    public AccountRepository(BankContext bankContext) : base(bankContext)
    {
    }

    public override async Task<Account> GetById(long key)
    {
        var account = await _bankContext.Accounts
            .Include(a => a.Customer)
            .SingleOrDefaultAsync(p => p.AccountNumber == key);

        return account ?? throw new Exception("No Account with the given ID");
    }


    public override async Task<IEnumerable<Account>> GetAll()
    {
        var Accounts = _bankContext.Accounts.Include(a => a.Customer);
        if (!Accounts.Any())
            throw new Exception("No Accounts in the database");
        return await Accounts.ToListAsync();
    }
}

