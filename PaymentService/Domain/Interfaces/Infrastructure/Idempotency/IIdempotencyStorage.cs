using Domain.Records.Response;

namespace Domain.Interfaces.Infrastructure.Idempotency
{
	public interface IIdempotencyStorage
	{
		Task<PaymentSendedResponse?> AlreadyProcessAsync(Guid key);
		Task SetNewProcessAsync(Guid key);
	}

}
