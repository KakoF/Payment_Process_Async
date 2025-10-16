using Domain.Records.CreatePayment;
using Domain.Records.Response;

namespace Domain.Interfaces.Application
{
	public interface IPaymentService
	{
        Task<PaymentSendedResponse> Handle(CreatePaymentRequest request, Guid idempotencyKey);
    }
}
