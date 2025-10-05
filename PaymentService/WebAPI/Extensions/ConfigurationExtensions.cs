using Infrastructure.Configurations;
using StackExchange.Redis;

namespace WebAPI.Extensions
{
	public static class ConfigurationExtensions
	{
        public static void AddConfigurations(this WebApplicationBuilder builder)
        {
			var kafkaConfig = builder.Configuration.GetSection("Kafka").Get<KafkaConfiguration>();
            builder.Services.AddSingleton(kafkaConfig!);

			builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
				ConnectionMultiplexer.Connect(builder.Configuration["Redis:Connection"]!));
		}
    }
}
