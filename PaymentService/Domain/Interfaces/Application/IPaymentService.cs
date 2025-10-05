using Domain.Records;
using Domain.Records.CreatePayment;

namespace Domain.Interfaces.Application
{
	public interface IPaymentService
	{
        Task<PaymentSendedResponse> Handle(CreatePaymentRequest request, Guid idempotencyKey);
    }
}
