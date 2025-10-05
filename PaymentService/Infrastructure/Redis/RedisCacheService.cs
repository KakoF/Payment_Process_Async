using Domain.Interfaces.Infrastructure.Redis;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Redis
{
	public class RedisCacheService : IRedisCacheService
	{
		private readonly IDatabase _db;
		private const string Prefix = "Payment:";

		public RedisCacheService(IConnectionMultiplexer redis)
		{
			_db = redis.GetDatabase();
		}

		public async Task SetAsync<T>(string key, T value, TimeSpan? ttl = null)
		{
			var json = JsonSerializer.Serialize(value);
			await _db.StringSetAsync($"{Prefix}{key}", json, ttl);
		}

		public async Task<T?> GetAsync<T>(string key)
		{
			var json = await _db.StringGetAsync($"{Prefix}{key}");
			if (json.IsNullOrEmpty) return default;

			return JsonSerializer.Deserialize<T>(json!);
		}

		public async Task<bool> ExistsAsync(string key)
		{
			return await _db.KeyExistsAsync($"{Prefix}{key}");
		}

		public async Task RemoveAsync(string key)
		{
			await _db.KeyDeleteAsync($"{Prefix}{key}");
		}
	}
}
