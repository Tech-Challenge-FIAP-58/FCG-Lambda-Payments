using MediatR;

namespace FCG.Lambda.Payments.SRC.Domain.Entities.Mediatr
{
    public class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}