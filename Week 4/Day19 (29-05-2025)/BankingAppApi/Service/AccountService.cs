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
        Customer customer = null;
        try
        {
            customer = await _customerRepository.GetById(dto.customerId);
        }
        catch (Exception)
        {
            throw new Exception("Account without a customer cannt be added");
        }
        Account? account = accountMapper.MapAccountAddRequestAccount(dto,customer);
        await _accountRepository.Add(account);
        return account;
    }
    public async Task<Account> updateBalanceAsync(int balance,long accountId)
    {
        Account account = null;
        try
        {
            account = await _accountRepository.GetById(accountId);
        }
        catch (Exception)
        {
            throw new Exception("Account is not valid");
        }
        account.Balance += balance;
        await _accountRepository.Update(accountId,account);
        return account;
    }

    public async Task<Account?> GetAccountByIdAsync(long accountId)
    {
        var account = await _accountRepository.GetById(accountId);
        return account;
    }

    public async Task<float> GetBalanceAsync(long accountId)
    {
        var account = await _accountRepository.GetById(accountId);
        var balance = account.Balance;
        return balance;
    }

    
}
