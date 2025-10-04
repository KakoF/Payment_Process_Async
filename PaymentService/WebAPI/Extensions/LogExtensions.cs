using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace WebAPI.Extensions
{
	public static class LogExtensions
	{
        public static void AddLog(this WebApplicationBuilder builder)
        {
			var otelUrl = builder.Configuration["Otel:Url"];
			builder.Logging.Configure(options =>
			{
				options.ActivityTrackingOptions = ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId;
			});
			builder.Logging.AddOpenTelemetry(logging =>
			{
				logging.IncludeFormattedMessage = true;
				logging.IncludeScopes = true;
				logging.AddOtlpExporter(options =>
				{
					options.Endpoint = new Uri(otelUrl!);
				});
			});
		}
    }
}
