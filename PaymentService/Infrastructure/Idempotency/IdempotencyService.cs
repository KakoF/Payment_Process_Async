using Domain.Interfaces.Infrastructure.Idempotency;
using Domain.Interfaces.Infrastructure.Redis;
using Domain.Records;

namespace Infrastructure.Idempotency
{
	public class IdempotencyService : IIdempotencyService
	{
		private readonly IRedisCacheService _cache;
		private const string Prefix = "Idempotency:";

		public IdempotencyService(IRedisCacheService cache)
		{
			_cache = cache;
		}

		public async Task<PaymentSended?> AlreadyProcessAsync(Guid key)
		{
			return await _cache.GetAsync<PaymentSended>(Prefix + key);
		}

		public async Task SetNewProcessAsync(Guid key)
		{
			var payment = PaymentSended.New(key);
			await _cache.SetAsync(Prefix + key, payment, TimeSpan.FromMinutes(1));
		}
	}

}
