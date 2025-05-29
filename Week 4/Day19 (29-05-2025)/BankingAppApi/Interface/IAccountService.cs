using System;
using BankingAppApi.Models;
using BankingAppApi.Models.DTO;

namespace BankingAppApi.Interface;

public interface IAccountService
{
    public Task<Account> AddAccountAsync(AccountAddRequestDTO account);

    public Task<Account?> GetAccountByIdAsync(long accountId);

    public Task<float> GetBalanceAsync(long accountId);
}
