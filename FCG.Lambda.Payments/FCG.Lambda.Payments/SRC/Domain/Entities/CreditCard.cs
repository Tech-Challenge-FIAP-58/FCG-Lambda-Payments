using FCG.Lambda.Payments.SRC.Domain.Events;

namespace FCG.Lambda.Payments.SRC.Domain.Entities;

public class CreditCard : Entity
{
    public string CardName { get; set; }
    public string CardNumber { get; set; }
    public string CardExpirationDate { get; set; }
    public string CVV { get; set; }

    protected CreditCard() { }

    public CreditCard(string cardName, string cardNumber, string cardExpirationDate, string cVV)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        CardExpirationDate = cardExpirationDate;
        CVV = cVV;
    }
}
