using System;
using BankingAppApi.Context;
using BankingAppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingAppApi.Repositoy;

public class TransactionRepository : Repository<int, Transaction>
{
    public TransactionRepository(BankContext bankContext) : base(bankContext)
    {
    }

    public override async Task<Transaction> GetById(int key)
    {
        var Transaction = await _bankContext.Transactions.SingleOrDefaultAsync(p => p.Id == key);

        return Transaction ?? throw new Exception("No Transaction with the given ID");
    }

    public override async Task<IEnumerable<Transaction>> GetAll()
    {
        var Transactions = _bankContext.Transactions;
        if (Transactions.Count() == 0)
            throw new Exception("No Transactions in the database");
        return await Transactions.ToListAsync();
    }
}
