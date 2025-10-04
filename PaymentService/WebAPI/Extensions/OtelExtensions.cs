using Confluent.Kafka.Extensions.OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace WebAPI.Extensions
{
	public static class OtelExtensions
	{
        public static void AddOtel(this WebApplicationBuilder builder)
        {
			var appName = "PaymentService";
			var otelUrl = builder.Configuration["Otel:Url"];

			var resourceBuilder = ResourceBuilder.CreateDefault().AddService(appName).AddAttributes(new[]
			{
				new KeyValuePair<string, object>("app", appName),
				new KeyValuePair<string, object>("env", builder.Environment.EnvironmentName),
				new KeyValuePair<string, object>("host.name", Environment.MachineName)
			});


			builder.Services.AddOpenTelemetry()
			 .WithMetrics(metrics => metrics
				 .SetResourceBuilder(resourceBuilder)
				 .AddAspNetCoreInstrumentation()
				 .AddHttpClientInstrumentation()
				 .AddRuntimeInstrumentation()
				 .AddProcessInstrumentation()
				 .AddEventCountersInstrumentation(options =>
				 {
					 options.AddEventSources("Microsoft.AspNetCore.Hosting", "System.Net.Http");
				 })
				 .AddMeter($"{appName}.*")
				 .AddOtlpExporter(options =>
				 {
					 options.Endpoint = new Uri(otelUrl);
				 }))
			 .WithTracing(tracing => tracing
				 .AddAspNetCoreInstrumentation()
				 .AddConfluentKafkaInstrumentation()
				 //.AddHttpClientInstrumentation()
				 .SetResourceBuilder(resourceBuilder)
				 .AddOtlpExporter(opt =>
				 {
					 opt.Endpoint = new Uri(otelUrl!);
					 opt.Protocol = OtlpExportProtocol.Grpc;
				 }));
		}
    }
}
