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
        if (customer == null || string.IsNullOrWhiteSpace(customer.Name))
        {
            throw new ArgumentException("Customer name cannot be empty or null.");
        }
        var newCustomer = new Customer
        {
            Name = customer.Name??""
        };

        newCustomer = await _customerRepository.Add(newCustomer);
        return newCustomer;
    }

    public async Task<List<Account>> GetAccountsByCustomerIdAsync(int customerId)
    {
        if (customerId <= 0)
        {
            throw new Exception("Customer ID must be a positive integer.");
        }

        var customer = await _customerRepository.GetById(customerId);
        if (customer == null)
        {
            throw new Exception($"Customer with ID {customerId} does not exist.");
        }

        var allAccounts = await _accountRepository.GetAll();
        var accounts = allAccounts.Where(a => a.CustomerId == customerId).ToList();

        return accounts;
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        if (customerId <= 0)
        {
            throw new Exception("Customer ID must be a positive integer.");
        }
        var customer = await _customerRepository.GetById(customerId);
        if (customer == null)
        {
            throw new Exception($"Customer with ID {customerId} not found.");
        }
        return customer;
    }
}
