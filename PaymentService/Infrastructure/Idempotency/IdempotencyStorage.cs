using Domain.Interfaces.Infrastructure.Idempotency;
using Domain.Interfaces.Infrastructure.Redis;
using Domain.Records.Response;

namespace Infrastructure.Idempotency
{
	public class IdempotencyStorage : IIdempotencyStorage
	{
		private readonly IRedis _cache;
		private const string Prefix = "Idempotency:";

		public IdempotencyStorage(IRedis cache)
		{
			_cache = cache;
		}

		public async Task<PaymentSendedResponse?> AlreadyProcessAsync(Guid key)
		{
			return await _cache.GetAsync<PaymentSendedResponse>(Prefix + key);
		}

		public async Task SetNewProcessAsync(Guid key)
		{
			var payment = PaymentSendedResponse.New(key);
			await _cache.SetAsync(Prefix + key, payment, TimeSpan.FromMinutes(1));
		}
	}

}
