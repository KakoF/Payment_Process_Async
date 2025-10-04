using Domain.Interfaces.Application;
using Domain.Interfaces.Infrastructure;
using Domain.Models;
using Domain.Records;

namespace Application.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IMessagePublisher _publisher;
        public PaymentService(IMessagePublisher publisher)
		{
			_publisher = publisher;
        }
		public async Task<PaymentSended> Handle(CreatePaymentRequest request)
		{
			var sender = PartyModel.Create(request.Sender.Name, request.Sender.Document);
			var receiver = PartyModel.Create(request.Receiver.Name, request.Receiver.Document);
            var model = PaymentModel.Create(request.Amount, sender, receiver);

            await _publisher.PublishAsync("payment.created", model);

			return PaymentSended.Success(model.PaymentId);
		}
	}
}
