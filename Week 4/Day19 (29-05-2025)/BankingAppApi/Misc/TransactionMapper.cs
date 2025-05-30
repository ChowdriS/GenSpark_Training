using System;
using BankingAppApi.Models;
using BankingAppApi.Models.DTO;

namespace BankingAppApi.Misc;

public class TransactionMapper
{
    public Transaction MapTransactionAddRequestTransaction(TransactionAddRequestDTO dto , string type, long FromAccountId, long ToAccountId)
        {
            Transaction Transaction = new();
            Transaction.Amount = dto.Amount;
            Transaction.TransactionType = type;
            Transaction.FromAccountId = FromAccountId;
            Transaction.ToAccountId = ToAccountId;
            return Transaction;
        }
    public Transaction MapTransactionWithDrawRequestTransaction(TransactionWithDrawRequestDTO dto , string type, long FromAccountId)
        {
            Transaction Transaction = new();
            Transaction.Amount = dto.Amount;
            Transaction.TransactionType = type;
            Transaction.FromAccountId = FromAccountId;
            return Transaction;
        }
    public Transaction MapTransactionDepositRequestTransaction(TransactionDepositRequestDTO dto , string type,long ToAccountId)
        {
            Transaction Transaction = new();
            Transaction.Amount = dto.Amount;
            Transaction.TransactionType = type;
            Transaction.ToAccountId = ToAccountId;
            return Transaction;
        }
}
