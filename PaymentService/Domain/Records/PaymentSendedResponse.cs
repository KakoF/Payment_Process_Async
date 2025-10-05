namespace Domain.Records
{
	public class PaymentSendedResponse
	{
		public Guid Id { get; set; }
		public string Status { get; set; } = null!;


		public static PaymentSendedResponse Success(Guid id)
		{
			return new PaymentSendedResponse() { Id = id, Status = "Payment success sended" };

        }

		public static PaymentSendedResponse New(Guid id)
		{
			return new PaymentSendedResponse() { Id = id, Status = "Payment already sended" };

		}
	}
}
