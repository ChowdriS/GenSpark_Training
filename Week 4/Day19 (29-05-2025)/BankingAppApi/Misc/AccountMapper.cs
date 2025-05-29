using System;
using BankingAppApi.Models;
using BankingAppApi.Models.DTO;

namespace BankingAppApi.Misc;

public class AccountMapper
{
    public Account? MapAccountAddRequestAccount(AccountAddRequestDTO dto, Customer customer)
    {
        Account Account = new();
        Account.AccountNumber = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
        Account.Balance = dto.Balance;
        Account.Customer = customer;
        return Account;
    }
}
