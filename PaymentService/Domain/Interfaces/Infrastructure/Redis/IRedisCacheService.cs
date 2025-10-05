
namespace Domain.Interfaces.Infrastructure.Redis
{
	public interface IRedisCacheService
	{
		Task SetAsync<T>(string key, T value, TimeSpan? ttl = null);
		Task<T?> GetAsync<T>(string key);
		Task<bool> ExistsAsync(string key);
		Task RemoveAsync(string key);

	}
}
