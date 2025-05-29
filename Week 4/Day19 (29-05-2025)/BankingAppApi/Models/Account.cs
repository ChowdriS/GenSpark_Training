using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingAppApi.Models;

public class Account
{
    [Key]
    public long AccountNumber { get; set; }

    public float Balance { get; set; }

    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }
}
