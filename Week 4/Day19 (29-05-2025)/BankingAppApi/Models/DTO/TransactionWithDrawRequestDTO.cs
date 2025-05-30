using System;

namespace BankingAppApi.Models.DTO;

public class TransactionWithDrawRequestDTO
{
    public float Amount { get; set; }
    public long FromAccountId { get; set; }

}
