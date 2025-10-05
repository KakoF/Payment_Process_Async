namespace Domain.Interfaces.Infrastructure.Brokers
{
	public interface IMessagePublisher
    {
        Task PublishAsync(string topic, object message);
    }
}
