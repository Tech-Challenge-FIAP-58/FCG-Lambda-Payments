using FCG.Lambda.Payments.SRC.Domain.Entities;

namespace FCG.Lambda.Payments.SRC.Facade
{
    public interface IPaymentFacade
    {
        Task<Transaction> ProcessPayment(Payment payment);
    }
}