
namespace Domain.Interfaces
{
	public interface IPaymentCreatedUseCase
	{
		void Handle(object message, CancellationToken cancellationToken);
	}
}
