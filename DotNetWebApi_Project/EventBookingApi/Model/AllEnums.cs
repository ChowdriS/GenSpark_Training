namespace EventBookingApi.Model;

public enum UserRole
{
    Admin = 0,
    User = 1,
    Manager = 2
}

public enum EventStatus
{
    Active = 0,
    Cancelled = 1,
    Completed = 2
}

public enum TicketStatus
{
    Booked = 0,
    Cancelled = 1,
    Used = 2
}

public enum TicketTypeEnum
{
    Regular = 0,
    VIP = 1,
    EarlyBird = 2
}

public enum PaymentTypeEnum
{
    Cash = 0,
    CreditCard = 1,
    DebitCard = 2,
    UPI = 3
}

public enum PaymentStatusEnum
{
    Paid = 0,
    Failed = 1,
    Pending = 2,
    Refund = 3
}

// no ticket number
// paymet when ticket booking
// refund before 2 days of event