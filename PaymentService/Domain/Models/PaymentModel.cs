namespace Domain.Models
{
    public class PaymentModel
    {
        public Guid PaymentId { get; private set; }
        public Guid IdempotencyKey { get; private set; }

        public decimal Amount { get; private set; }

        public DateTime CreatetAt { get; private set; }

        public PartyModel Sender { get; private set; } = null!;

        public PartyModel Receiver { get; private set; } = null!;
        private PaymentModel(Guid paymentId, Guid idempotencyKey, decimal amount, DateTime createtAt, PartyModel sender, PartyModel receiver)
        {
            PaymentId = paymentId;
            IdempotencyKey = idempotencyKey;
			Amount = amount;
            CreatetAt = createtAt;
            Sender = sender;
            Receiver = receiver;
        }

        public static PaymentModel Create(Guid idempotencyKey, decimal amount, PartyModel sender, PartyModel receiver)
        {

            return new PaymentModel(Guid.NewGuid(), idempotencyKey, amount, DateTime.Now, sender, receiver);
        }
    }
}
