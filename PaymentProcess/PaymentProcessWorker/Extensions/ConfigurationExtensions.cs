using Infrastructure.Configurations;

namespace PaymentProcessWorker.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void AddConfigurations(this HostApplicationBuilder builder)
        {
			builder.Services.Configure<KafkaConfiguration>(builder.Configuration.GetSection("Kafka"));
        }
    }
}
