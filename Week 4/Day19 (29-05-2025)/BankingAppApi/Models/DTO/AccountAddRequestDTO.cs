using System;

namespace BankingAppApi.Models.DTO;

public class AccountAddRequestDTO
{

    public float Balance { get; set; }

    public int customerId{ get; set; }
}
