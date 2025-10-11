using Confluent.Kafka;
using Domain.Interfaces.Infrastructure.Brokers;
using Infrastructure.Configurations;
using Infrastructure.Meters;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
namespace Infrastructure.Brokers.Publishers
{
	public class KafkaPublisher : IMessagePublisher
    {
        private readonly IProducer<string?, string> _producer;

        public KafkaPublisher(KafkaConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration.BootstrapServers
            };

            _producer = new ProducerBuilder<string?, string>(config).Build();
        }

        public async Task PublishAsync(string topic, object message, string? key = null)
        {
            using var activity = TelemetrySources.PaymentServicePublisher.StartActivity("PublishPayment", ActivityKind.Producer);

            activity?.SetTag("messaging.system", "kafka");
            activity?.SetTag("messaging.destination", topic);
            activity?.SetTag("messaging.destination_kind", "topic");
            activity?.SetTag("messaging.kafka.message_key", key);
            activity?.SetTag("messaging.operation", "send");

            var headers = new Headers();

            if (activity != null)
            {
                headers.Add("traceparent", Encoding.UTF8.GetBytes(activity.Id));

                foreach (var baggage in activity.Baggage)
                {
                    headers.Add($"baggage-{baggage.Key}", Encoding.UTF8.GetBytes(baggage.Value));
                }
            }

            var kafkaMessage = new Message<string?, string>
            {
                Key = key,
                Value = JsonSerializer.Serialize(message),
                Headers = headers

            };

            var result = await _producer.ProduceAsync(topic, kafkaMessage);
            activity?.SetStatus(ActivityStatusCode.Ok);
            activity?.SetTag("messaging.kafka.partition", result.Partition.Value);
            activity?.SetTag("messaging.kafka.offset", result.Offset.Value);

        }
    }
}
