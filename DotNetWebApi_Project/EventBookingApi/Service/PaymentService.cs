using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;
using EventBookingApi.Interface;

namespace EventBookingApi.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Guid, Payment> _paymentRepository;
        private readonly IRepository<Guid, Ticket> _ticketRepository;
        private readonly IRepository<Guid, Event> _eventRepository;
        private readonly IRepository<Guid, User> _userRepository;

        public PaymentService(
            IRepository<Guid, Payment> paymentRepository,
            IRepository<Guid, Ticket> ticketRepository,
            IRepository<Guid, Event> eventRepository,
            IRepository<Guid, User> userRepository)
        {
            _paymentRepository = paymentRepository;
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
        }

        public async Task<PaymentDetailDTO> GetPaymentById(Guid paymentId, Guid? requesterId)
        {
            var payment = await _paymentRepository.GetById(paymentId);
            if (payment == null) throw new Exception("Payment not found");

            var ticket = await _ticketRepository.GetById(payment.TicketId);
            if (ticket == null) throw new Exception("Ticket not found");

            if (requesterId.HasValue)
            {
                var requester = await _userRepository.GetById(requesterId.Value);
                
                if (requester.Role == UserRole.User && ticket.UserId != requesterId)
                    throw new UnauthorizedAccessException("Access denied");
            }

            var eventObj = await _eventRepository.GetById(ticket.EventId);
            var user = await _userRepository.GetById(ticket.UserId);

            return new PaymentDetailDTO
            {
                Id = payment.Id,
                PaymentType = payment.PaymentType,
                Amount = payment.Amount,
                Status = payment.PaymentStatus,
                TransactionId = payment.TransactionId,
                EventId = eventObj.Id,
                EventTitle = eventObj.Title ?? "",
                UserId = user.Id,
                UserName = user.Username??"",
                UserEmail = user.Email??"",
                BookedAt = ticket.BookedAt,
                TicketStatus = ticket.Status
            };
        }

        public async Task<IEnumerable<PaymentDetailDTO>> GetPaymentsByUserId(Guid userId)
        {
            var tickets = (await _ticketRepository.GetAll())
                .Where(t => t.UserId == userId)
                .ToList();

            var ticketIds = tickets.Select(t => t.Id).ToList();
            var payments = (await _paymentRepository.GetAll())
                .Where(p => ticketIds.Contains(p.TicketId))
                .ToList();

            var eventIds = tickets.Select(t => t.EventId).Distinct().ToList();
            var events = (await _eventRepository.GetAll())
                .Where(e => eventIds.Contains(e.Id))
                .ToDictionary(e => e.Id);

            var user = await _userRepository.GetById(userId);

            return payments.Select(payment =>
            {
                var ticket = tickets.First(t => t.Id == payment.TicketId);
                var eventObj = events[ticket.EventId];

                return new PaymentDetailDTO
                {
                    Id = payment.Id,
                    PaymentType = payment.PaymentType,
                    Amount = payment.Amount,
                    Status = payment.PaymentStatus,
                    TransactionId = payment.TransactionId,
                    EventId = eventObj.Id,
                    EventTitle = eventObj.Title ?? "",
                    UserId = user.Id,
                    UserName = user.Username ?? "",
                    UserEmail = user.Email ?? "",
                    BookedAt = ticket.BookedAt,
                    TicketStatus = ticket.Status
                };
            });
        }

        public async Task<IEnumerable<PaymentDetailDTO>> GetPaymentsByEventId(Guid eventId, Guid? managerId)
        {
            var eventObj = await _eventRepository.GetById(eventId);
            if (eventObj == null || eventObj.IsDeleted) 
                throw new Exception("Event not found");

            if (managerId.HasValue)
            {
                if (eventObj.ManagerId != managerId.Value)
                    throw new UnauthorizedAccessException("Access denied");
            }

            var tickets = (await _ticketRepository.GetAll())
                .Where(t => t.EventId == eventId)
                .ToList();

            var ticketIds = tickets.Select(t => t.Id).ToList();
            var payments = (await _paymentRepository.GetAll())
                .Where(p => ticketIds.Contains(p.TicketId))
                .ToList();

            var userIds = tickets.Select(t => t.UserId).Distinct().ToList();
            var users = (await _userRepository.GetAll())
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id);

            return payments.Select(payment =>
            {
                var ticket = tickets.First(t => t.Id == payment.TicketId);
                var user = users[ticket.UserId];

                return new PaymentDetailDTO
                {
                    Id = payment.Id,
                    PaymentType = payment.PaymentType,
                    Amount = payment.Amount,
                    Status = payment.PaymentStatus,
                    TransactionId = payment.TransactionId,
                    EventId = eventObj.Id,
                    EventTitle = eventObj.Title ?? "",
                    UserId = user.Id,
                    UserName = user.Username ?? "",
                    UserEmail = user.Email ?? "",
                    BookedAt = ticket.BookedAt,
                    TicketStatus = ticket.Status
                };
            });
        }
    }
}