using Confluent.Kafka;
using Domain.Interfaces.Infrastructure;
using Infrastructure.Configurations;
using System.Text.Json;
namespace Infrastructure.Brokers.Publishers
{
	public class KafkaPublisher : IMessagePublisher
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaPublisher(KafkaConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration.BootstrapServers
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task PublishAsync(string topic, object message)
        {
            string stringMessage = JsonSerializer.Serialize(message);
            var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = stringMessage });
            Console.WriteLine($"Mensagem publicada: {result.Value} em {result.TopicPartitionOffset}");
        }
	}
}
