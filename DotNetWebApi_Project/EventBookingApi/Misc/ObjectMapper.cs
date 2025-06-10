using System;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Misc;

public class ObjectMapper
{
    public TicketResponseDTO TicketResponseDTOMapper(Ticket ticket, Event eventObj, TicketType ticketType, Payment? payment) => new()
    {
        Id = ticket.Id,
        EventTitle = eventObj?.Title ?? "",
        TicketType = ticketType?.TypeName.ToString() ?? "",
        PricePerTicket = ticketType?.Price ?? 0,
        Quantity = ticket.BookedQuantity,
        BookedAt = ticket.BookedAt,
        Payment = payment != null ? new PaymentResponseDTO
        {
            Id = payment.Id,
            PaymentType = payment.PaymentType,
            Amount = payment.Amount,
            Status = payment.PaymentStatus,
            TransactionId = payment.TransactionId
        } : null
    };
}
