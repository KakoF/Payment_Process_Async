using Domain.Interfaces.Application;
using Domain.Interfaces.Infrastructure;
using Domain.Models;
using Domain.Records;
using Infrastructure.Meters;

namespace Application.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IMessagePublisher _publisher;
		private readonly MetricsHelper _metrics;
		public PaymentService(IMessagePublisher publisher, MetricsHelper metricsHelper)
		{
			_publisher = publisher;
			_metrics = metricsHelper;
        }
		public async Task<PaymentSended> Handle(CreatePaymentRequest request)
		{
			var sender = PartyModel.Create(request.Sender.Name, request.Sender.Document);
			var receiver = PartyModel.Create(request.Receiver.Name, request.Receiver.Document);
            var model = PaymentModel.Create(request.Amount, sender, receiver);

			_metrics.RecordCounter("payment.created"); 

			await _publisher.PublishAsync("payment.created", model);

			return PaymentSended.Success(model.PaymentId);
		}
	}
}
