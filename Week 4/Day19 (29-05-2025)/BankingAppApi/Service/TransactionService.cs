using System;
using BankingAppApi.Context;
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
    private readonly IRepository<long, Account> _accountRepository;
    private readonly TransactionMapper transactionMapper;
    private readonly BankContext _bankContext;

    public TransactionService(
        IRepository<int, Transaction> transactionRepository,
        IRepository<long, Account> accountRepository,
        BankContext bankContext 
    )
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _bankContext = bankContext;
        transactionMapper = new();
    }

    public async Task<Transaction> DepositAsync(TransactionDepositRequestDTO dto)
    {
        if (dto == null || dto.Amount <= 0 || dto.ToAccountId <= 0)
            throw new Exception("Invalid deposit request.");

        await using var transaction = await _bankContext.Database.BeginTransactionAsync();

        try
        {
            var toAccount = await _accountRepository.GetById(dto.ToAccountId)
                ?? throw new Exception("Destination account not found.");

            var txn = transactionMapper.MapTransactionDepositRequestTransaction(dto, "credit", dto.ToAccountId);

            txn = await _transactionRepository.Add(txn);

            toAccount.Balance += dto.Amount;
            await _accountRepository.Update(toAccount.AccountNumber, toAccount);

            await transaction.CommitAsync();
            return txn;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Transaction> WithdrawAsync(TransactionWithDrawRequestDTO dto)
    {
        if (dto == null || dto.Amount <= 0 || dto.FromAccountId <= 0)
            throw new Exception("Invalid withdraw request.");

        await using var transaction = await _bankContext.Database.BeginTransactionAsync();

        try
        {
            var fromAccount = await _accountRepository.GetById(dto.FromAccountId)
                ?? throw new Exception("Source account not found.");

            if (fromAccount.Balance < dto.Amount)
                throw new Exception("Insufficient balance.");

            var txn = transactionMapper.MapTransactionWithDrawRequestTransaction(dto, "debit", dto.FromAccountId);

            txn = await _transactionRepository.Add(txn);

            fromAccount.Balance -= dto.Amount;
            await _accountRepository.Update(fromAccount.AccountNumber, fromAccount);

            await transaction.CommitAsync();
            return txn;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Transaction> TransferAsync(TransactionAddRequestDTO dto)
    {
        if (dto == null || dto.Amount <= 0 || dto.FromAccountId <= 0 || dto.ToAccountId <= 0)
            throw new Exception("Invalid transfer request.");

        if (dto.FromAccountId == dto.ToAccountId)
            throw new Exception("Cannot transfer to the same account.");

        await using var transaction = await _bankContext.Database.BeginTransactionAsync();

        try
        {
            var fromAccount = await _accountRepository.GetById(dto.FromAccountId)
                ?? throw new Exception("Sender account not found.");

            var toAccount = await _accountRepository.GetById(dto.ToAccountId)
                ?? throw new Exception("Receiver account not found.");

            if (fromAccount.Balance < dto.Amount)
                throw new Exception("Insufficient balance for transfer.");

            var txn = transactionMapper.MapTransactionAddRequestTransaction(dto, "transfer", dto.FromAccountId, dto.ToAccountId);
            txn = await _transactionRepository.Add(txn);

            fromAccount.Balance -= dto.Amount;
            toAccount.Balance += dto.Amount;

            await _accountRepository.Update(fromAccount.AccountNumber, fromAccount);
            await _accountRepository.Update(toAccount.AccountNumber, toAccount);

            await transaction.CommitAsync();
            return txn;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(long accountId)
    {
        if (accountId <= 0) throw new Exception("Invalid account ID.");
        var all = await _transactionRepository.GetAll();
        return all.Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId).ToList();
    }
}