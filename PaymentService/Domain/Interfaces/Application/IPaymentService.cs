using Domain.Records;

namespace Domain.Interfaces.Application
{
	public interface IPaymentService
	{
        Task<PaymentSended> Handle(CreatePaymentRequest request);
    }
}
