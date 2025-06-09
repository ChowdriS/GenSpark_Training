using System;
using System.ComponentModel.DataAnnotations;

namespace EventBookingApi.Model.DTO;

public class PaymentRequestDTO
{
    [Required]
    public PaymentTypeEnum PaymentType { get; set; }
    
    [Required]
    public Guid TransactionId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive")]
    public decimal Amount { get; set; }
}