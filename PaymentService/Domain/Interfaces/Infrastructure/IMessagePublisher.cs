namespace Domain.Interfaces.Infrastructure
{
	public interface IMessagePublisher
    {
        Task PublishAsync(string topic, object message);
    }
}
