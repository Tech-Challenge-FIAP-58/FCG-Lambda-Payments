using FCG.Lambda.Payments.Core.Integration;
using FCG.Lambda.Payments.SRC.Domain.Entities.Mediatr;

namespace FCG.Lambda.Payments.SRC.Domain;

public class PaymentProcessedDomainEvent : Event
{
    public Guid OrderId { get; }
    public Guid PaymentId { get; }
    public decimal Amount { get; }
    public PaymentResultStatus Status { get; }
    public string? Reason { get; }

    public PaymentProcessedDomainEvent(
        Guid orderId,
        Guid paymentId,
        decimal amount,
        PaymentResultStatus status,
        string? reason = null)
    {
        OrderId = orderId;
        PaymentId = paymentId;
        Amount = amount;
        Status = status;
        Reason = reason;
    }
}
