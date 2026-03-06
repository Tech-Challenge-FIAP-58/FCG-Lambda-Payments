namespace FCG.Lambda.Payments.SRC.Domain.Entities.Enums;

public enum TransactionStatus
{
    Authorized = 1,
    Paid,
    Declined,
    Refunded,
    Cancelled
}
