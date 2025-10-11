using Confluent.Kafka;
using Domain.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.TelemetryUtils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Brokers.Consumers
{
    public class PaymentCreatedConsumer : BackgroundService
    {
        private readonly KafkaConfiguration _config;
        private readonly ILogger<PaymentCreatedConsumer> _logger;
        private readonly IPaymentCreatedUseCase _paymentCreatedUseCase;
		


		public PaymentCreatedConsumer(IOptions<KafkaConfiguration> config, ILogger<PaymentCreatedConsumer> logger, IPaymentCreatedUseCase paymentCreatedUseCase)
        {
            _config = config.Value;
            _logger = logger;
            _paymentCreatedUseCase = paymentCreatedUseCase;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _config.BootstrapServers,
                GroupId = _config.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe(_config.Topic);

            while (!stoppingToken.IsCancellationRequested)
            {
               
					var result = consumer.Consume(stoppingToken);

					ActivityContext parentContext = default;
					var traceParentHeader = result.Message.Headers.FirstOrDefault(h => h.Key == "traceparent");
					if (traceParentHeader != null)
					{
						var traceParent = Encoding.UTF8.GetString(traceParentHeader.GetValueBytes());
						parentContext = ActivityContext.Parse(traceParent, null);
					}

					using var activity = TelemetrySources.PaymentCreatedConsumer.StartActivity("ConsumePayment", ActivityKind.Consumer, parentContext);

					activity?.SetTag("messaging.system", "kafka");
					activity?.SetTag("messaging.destination", result.Topic);
					activity?.SetTag("messaging.kafka.message_key", result.Message.Key);
					activity?.SetTag("messaging.kafka.partition", result.Partition.Value);
					activity?.SetTag("messaging.kafka.offset", result.Offset.Value);

				try
				{
					var objectMessage = JsonSerializer.Deserialize<object>(result.Message.Value);

					_paymentCreatedUseCase.Handle(objectMessage!, stoppingToken);

				}
				catch (ConsumeException ex)
                {
					activity?.SetStatus(ActivityStatusCode.Error);
					activity?.SetTag("exception.type", ex.GetType().Name);
					activity?.SetTag("exception.message", ex.Message);
					activity?.SetTag("exception.stacktrace", ex.StackTrace);

					_logger.LogError($"Erro ao processar pagamento: {ex.Error.Reason}");
                }
            }

            consumer.Close();
            return Task.CompletedTask;
        }
    }
}
