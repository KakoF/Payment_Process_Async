
using Domain.Records;

namespace Domain.Interfaces.Infrastructure.Idempotency
{
	public interface IIdempotencyService
	{
		Task<PaymentSended?> AlreadyProcessAsync(Guid key);
		Task SetNewProcessAsync(Guid key);
	}

}
