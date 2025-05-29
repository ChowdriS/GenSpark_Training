using System;
using System.ComponentModel.DataAnnotations;

namespace BankingAppApi.Models;

public class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Account>? Accounts { get; set; }
}
