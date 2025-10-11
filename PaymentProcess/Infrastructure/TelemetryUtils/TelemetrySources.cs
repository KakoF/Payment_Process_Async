using System.Diagnostics;
namespace Infrastructure.TelemetryUtils
{
	public static class TelemetrySources
	{
		public static readonly ActivitySource PaymentCreatedConsumer = new("PaymentProcess");
	}

}
