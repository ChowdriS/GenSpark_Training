using System;

namespace BankingAppApi.Models.DTO;

public class TransactionDepositRequestDTO
{
    public float Amount { get; set; }
    public long ToAccountId { get; set; }
}
