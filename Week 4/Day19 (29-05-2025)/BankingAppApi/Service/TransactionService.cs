using System;
using BankingAppApi.Interface;
using BankingAppApi.Misc;
using BankingAppApi.Models;
using BankingAppApi.Models.DTO;
using BankingAppApi.Repositoy;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BankingAppApi.Service;

public class TransactionService : ITransactionService
{
    private readonly IRepository<int, Transaction> _transactionRepository;
    private readonly TransactionMapper transactionMapper;
    private readonly IRepository<long, Account> _accountRepository;

    public TransactionService(IRepository<int, Transaction> transactionRepository, IRepository<long, Account> accountRepository)
    {
        transactionMapper = new();
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }
    public async Task<Transaction> DepositAsync(TransactionAddRequestDTO dto)
    {
        Transaction? transaction = transactionMapper.MapTransactionAddRequestTransaction(dto, "credit", null, dto.ToAccountId);
        transaction = await _transactionRepository.Add(transaction);
        await UpdateBalanceAsync(dto.ToAccountId??0,1*dto.Amount);
        return transaction;
    }

    public async Task<Transaction> TransferAsync(TransactionAddRequestDTO dto)
    {
        Transaction? transaction = transactionMapper.MapTransactionAddRequestTransaction(dto, "transfer", dto.FromAccountId, dto.ToAccountId);
        transaction = await _transactionRepository.Add(transaction);
        await UpdateBalanceAsync(dto.FromAccountId??0,-1*dto.Amount);
        await UpdateBalanceAsync(dto.ToAccountId??0,1*dto.Amount);
        return transaction;
    }

    public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(long accountId)
    {
        var allTransactions = await _transactionRepository.GetAll();
        var matchedTransactions = allTransactions.Where(trans => trans.FromAccountId == accountId || trans.ToAccountId == accountId);
        return matchedTransactions.ToList();
    }

    public async Task<Transaction> WithdrawAsync(TransactionAddRequestDTO dto)
    {
        Transaction? transaction = transactionMapper.MapTransactionAddRequestTransaction(dto, "debit", dto.FromAccountId, null);
        transaction = await _transactionRepository.Add(transaction);
        
        await UpdateBalanceAsync(dto.FromAccountId??0,-1*dto.Amount);
        return transaction;
    }
    
    public async Task<bool> UpdateBalanceAsync(long accountId, float newBalance)
    {
        Account account = await _accountRepository.GetById(accountId);
        account.Balance += newBalance;
        try
        {
            await _accountRepository.Update(accountId, account);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
