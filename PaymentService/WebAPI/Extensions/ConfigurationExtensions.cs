using Infrastructure.Configurations;

namespace WebAPI.Extensions
{
	public static class ConfigurationExtensions
	{
        public static void AddConfigurations(this WebApplicationBuilder builder)
        {
            var teste = builder.Configuration.GetSection("Kafka");

			var kafkaConfig = builder.Configuration.GetSection("Kafka").Get<KafkaConfiguration>();
            builder.Services.AddSingleton(kafkaConfig!);
        }
    }
}
