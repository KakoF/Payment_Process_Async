namespace Domain.Records
{
	public class PaymentSended
	{
		public Guid Id { get; set; }
		public string Status { get; set; } = null!;


		public static PaymentSended Success(Guid id)
		{
			return new PaymentSended() { Id = id, Status = "Payment success sended" };

        }

		public static PaymentSended New(Guid id)
		{
			return new PaymentSended() { Id = id, Status = "Payment already sended" };

		}
	}
}
