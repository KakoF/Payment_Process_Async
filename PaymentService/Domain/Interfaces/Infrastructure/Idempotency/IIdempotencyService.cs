
using Domain.Records;

namespace Domain.Interfaces.Infrastructure.Idempotency
{
	public interface IIdempotencyService
	{
		Task<PaymentSendedResponse?> AlreadyProcessAsync(Guid key);
		Task SetNewProcessAsync(Guid key);
	}

}
