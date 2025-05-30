using System;
using BankingAppApi.Models;
using BankingAppApi.Models.DTO;

namespace BankingAppApi.Interface;

public interface ITransactionService
{
    public Task<Transaction> DepositAsync(TransactionDepositRequestDTO dto);

    public Task<Transaction> WithdrawAsync(TransactionWithDrawRequestDTO dto);
    
    public Task<List<Transaction>> GetTransactionsByAccountIdAsync(long accountId);

    public Task<Transaction> TransferAsync(TransactionAddRequestDTO dto);
}
