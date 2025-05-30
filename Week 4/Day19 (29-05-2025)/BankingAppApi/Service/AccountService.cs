using System;
using BankingAppApi.Interface;
using BankingAppApi.Misc;
using BankingAppApi.Models;
using BankingAppApi.Models.DTO;
using BankingAppApi.Repositoy;

namespace BankingAppApi.Service;

public class AccountService : IAccountService
{
    private readonly IRepository<long, Account> _accountRepository;
    private readonly IRepository<int, Customer> _customerRepository;
    private AccountMapper accountMapper;
    public AccountService(IRepository<long, Account> accountRepository, IRepository<int, Customer> customerRepository)
    {
        _accountRepository = accountRepository;
        _customerRepository = customerRepository;
        accountMapper = new();
    }
    public async Task<Account> AddAccountAsync(AccountAddRequestDTO dto)
    {
        if (dto == null)
            throw new Exception("Account data must be provided.");
        Customer customer;
        try
        {
            customer = await _customerRepository.GetById(dto.customerId);
        }
        catch (Exception)
        {
            throw new Exception("Account cannot be added without a valid customer.");
        }
        var account = accountMapper.MapAccountAddRequestAccount(dto, customer);
        if (account == null)
            throw new Exception("Account mapping failed.");
        await _accountRepository.Add(account);
        return account;
    }
    public async Task<Account> updateBalanceAsync(int balance,long accountId)
    {
        Account account;
        try
        {
            account = await _accountRepository.GetById(accountId);
        }
        catch (Exception)
        {
            throw new Exception("Account is not valid.");
        }
        account.Balance += balance;
        await _accountRepository.Update(accountId, account);
        return account;
    }

    public async Task<Account?> GetAccountByIdAsync(long accountId)
    {
        try
        {
            var account = await _accountRepository.GetById(accountId);
            return account;
        }
        catch (Exception)
        {
            throw new Exception($"Account with ID {accountId} not found.");
        }

    }

    public async Task<float> GetBalanceAsync(long accountId)
    {
        try
        {
            var account = await _accountRepository.GetById(accountId);
            return account.Balance;
        }
        catch (Exception)
        {
            throw new Exception($"Account with ID {accountId} not found.");
        }
    }

    
}
