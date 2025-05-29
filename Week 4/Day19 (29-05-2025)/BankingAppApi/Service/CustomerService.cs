using System;
using BankingAppApi.Interface;
using BankingAppApi.Models;
using BankingAppApi.Models.DTO;
using BankingAppApi.Repositoy;

namespace BankingAppApi.Service;

public class CustomerService : ICustomerService
{
    private readonly IRepository<long, Account> _accountRepository;
    private readonly IRepository<int, Customer> _customerRepository;

    public CustomerService(IRepository<int, Customer> customerRepository, IRepository<long, Account> accountRepository)
    {
        _customerRepository = customerRepository;
        _accountRepository = accountRepository;
    }
    public async Task<Customer> AddCustomerAsync(CustomerAddRequestDTO customer)
    {
        var newCustomer = new Customer
        {
            Name = customer.Name??""
        };

        newCustomer = await _customerRepository.Add(newCustomer);
        return newCustomer;
    }

    public async Task<List<Account>> GetAccountsByCustomerIdAsync(int customerId)
    {
        var allAccounts = await _accountRepository.GetAll();
        var Accounts = allAccounts.Where(a => a.CustomerId == customerId).ToList();
        return Accounts;
        //     var allCustomer = await _customerRepository.GetAll();
        //     var accounts = allCustomer.Where(c => c.Id == customerId)
        //                               .Select(c => c.Accounts)
        //                               .ToList();
        //     return accounts;
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        var Customer = await _customerRepository.GetById(customerId);
        return Customer;
    }
}
