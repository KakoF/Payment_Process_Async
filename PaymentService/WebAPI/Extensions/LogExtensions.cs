using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace WebAPI.Extensions
{
	public static class LogExtensions
	{
        public static void AddLog(this WebApplicationBuilder builder)
        {
			var appName = "PaymentService";
			var otelUrl = builder.Configuration["Otel:Url"];

			var resourceBuilder = ResourceBuilder.CreateDefault()
			.AddService(appName)
			.AddAttributes(new[]
			{
				new KeyValuePair<string, object>("app", appName),
				new KeyValuePair<string, object>("env", builder.Environment.EnvironmentName),
				new KeyValuePair<string, object>("host.name", Environment.MachineName)
			});


			builder.Logging.Configure(options =>
			{
				options.ActivityTrackingOptions = ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId;
			});
			builder.Logging.AddOpenTelemetry(logging =>
			{
				logging.IncludeFormattedMessage = true;
				logging.IncludeScopes = true;
				logging.SetResourceBuilder(resourceBuilder);
				logging.AddOtlpExporter(options =>
				{
					options.Endpoint = new Uri(otelUrl!);
				});
			});
		}
    }
}
