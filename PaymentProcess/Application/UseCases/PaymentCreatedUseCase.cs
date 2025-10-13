using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases
{
	public class PaymentCreatedUseCase : IPaymentCreatedUseCase
	{
		private readonly ILogger<PaymentCreatedUseCase> _logger;

		public PaymentCreatedUseCase(ILogger<PaymentCreatedUseCase> logger)
		{
			_logger = logger;
		}
		public void Handle(object message, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Start process Payment");
			Console.WriteLine(message);
		}
	}
}
