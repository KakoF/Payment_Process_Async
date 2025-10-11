using Domain.Interfaces;

namespace Application.UseCases
{
	public class PaymentCreatedUseCase : IPaymentCreatedUseCase
	{
		public void Handle(object message, CancellationToken cancellationToken)
		{
			Console.WriteLine(message);
		}
	}
}
