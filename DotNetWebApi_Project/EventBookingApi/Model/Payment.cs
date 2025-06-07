using System;
using System.ComponentModel.DataAnnotations;

namespace EventBookingApi.Model;

public class Payment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TicketId { get; set; }
    public Ticket? Ticket { get; set; }

    public decimal Amount { get; set; }

    public PaymentTypeEnum PaymentType { get; set; } = PaymentTypeEnum.CreditCard;

    public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Paid;

    public string? TransactionId { get; set; }

    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
}