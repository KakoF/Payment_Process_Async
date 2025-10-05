using FluentValidation;

namespace Domain.Records.CreatePayment
{
	public class CreatePaymentValidator : AbstractValidator<CreatePaymentRequest>
	{
		public CreatePaymentValidator()
		{
			RuleFor(a => a.Amount).NotNull().GreaterThan(0);
			RuleFor(a => a.Sender.Name).NotEmpty().NotNull().MinimumLength(3);
			RuleFor(a => a.Sender.Document).NotEmpty().NotNull().MinimumLength(3);
			RuleFor(a => a.Receiver.Name).NotEmpty().NotNull().MinimumLength(3);
			RuleFor(a => a.Receiver.Document).NotEmpty().NotNull().MinimumLength(3);
		}
	}
}
