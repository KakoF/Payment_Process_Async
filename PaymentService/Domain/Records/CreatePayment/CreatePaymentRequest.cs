using Domain.Models;

namespace Domain.Records.CreatePayment
{
	public record CreatePaymentRequest(decimal Amount, PartyRequest Sender, PartyRequest Receiver);

    public record PartyRequest(string Name, string Document);
}
