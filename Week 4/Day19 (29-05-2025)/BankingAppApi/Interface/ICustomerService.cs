using System;
using BankingAppApi.Models;
using BankingAppApi.Models.DTO;

namespace BankingAppApi.Interface;

public interface ICustomerService
{
    public Task<Customer> AddCustomerAsync(CustomerAddRequestDTO customer);

    public Task<Customer?> GetCustomerByIdAsync(int customerId);

    public Task<List<Account>> GetAccountsByCustomerIdAsync(int customerId);
}
