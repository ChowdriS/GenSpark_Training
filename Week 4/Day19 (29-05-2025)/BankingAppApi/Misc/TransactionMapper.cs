using System;
using BankingAppApi.Models;
using BankingAppApi.Models.DTO;

namespace BankingAppApi.Misc;

public class TransactionMapper
{
    public Transaction? MapTransactionAddRequestTransaction(TransactionAddRequestDTO dto , string type, long? FromAccountId, long? ToAccountId)
        {
            Transaction Transaction = new();
            Transaction.Amount = dto.Amount;
            Transaction.TransactionType = type;
            Transaction.FromAccountId = FromAccountId;
            Transaction.ToAccountId = ToAccountId;
            return Transaction;
        }
}
