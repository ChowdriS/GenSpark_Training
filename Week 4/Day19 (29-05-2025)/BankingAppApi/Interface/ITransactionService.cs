using System;
using BankingAppApi.Models;
using BankingAppApi.Models.DTO;

namespace BankingAppApi.Interface;

public interface ITransactionService
{
    public Task<Transaction> DepositAsync(TransactionAddRequestDTO dto);

    public Task<Transaction> WithdrawAsync(TransactionAddRequestDTO dto);
    
    public Task<List<Transaction>> GetTransactionsByAccountIdAsync(long accountId);

    public Task<Transaction> TransferAsync(TransactionAddRequestDTO dto);
}
