using Confluent.Kafka;
using Domain.Interfaces;
using Infrastructure.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
                try
                {
					var result = consumer.Consume(stoppingToken);
					var objectMessage = JsonSerializer.Deserialize<object>(result.Message.Value);

					_paymentCreatedUseCase.Handle(objectMessage!, stoppingToken);

				}
				catch (ConsumeException ex)
                {
                    _logger.LogError($"Erro ao processar pagamento: {ex.Error.Reason}");
                }
            }

            consumer.Close();
            return Task.CompletedTask;
        }
    }
}
