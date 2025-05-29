using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingAppApi.Models;

public class Transaction
{
    public int Id { get; set; }

    public float Amount { get; set; }

    public string TransactionType { get; set; } = string.Empty;

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    public long? FromAccountId { get; set; }
    public Account? FromAccount { get; set; }

    public long? ToAccountId { get; set; }
    public Account? ToAccount { get; set; }
}


