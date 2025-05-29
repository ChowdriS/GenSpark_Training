using System;

namespace BankingAppApi.Models.DTO;

public class TransactionAddRequestDTO
{
    public float Amount { get; set; }
    public long? FromAccountId { get; set; }

    public long? ToAccountId { get; set; }
}
