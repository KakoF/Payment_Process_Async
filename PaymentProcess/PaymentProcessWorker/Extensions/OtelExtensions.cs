using Confluent.Kafka.Extensions.OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


namespace PaymentProcessWorker.Extensions
{
    public static class OtelExtensions
    {
        public static void AddOtel(this HostApplicationBuilder builder)
        {
            var appName = "PaymentProcess";
            var otelUrl = builder.Configuration["Otel:Url"];

            var resourceBuilder = ResourceBuilder.CreateDefault().AddService(appName).AddAttributes(new[]
            {
                new KeyValuePair<string, object>("app", appName),
                new KeyValuePair<string, object>("env", builder.Environment.EnvironmentName),
                new KeyValuePair<string, object>("host.name", Environment.MachineName)
            });


            builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
			     .AddSource("PaymentProcess")
				 .AddAspNetCoreInstrumentation()
                 .AddConfluentKafkaInstrumentation()
                 .SetResourceBuilder(resourceBuilder)
                 .AddOtlpExporter(opt =>
                 {
                     opt.Endpoint = new Uri(otelUrl!);
                     opt.Protocol = OtlpExportProtocol.Grpc;
                 }))
             .WithMetrics(metrics => metrics
                 .SetResourceBuilder(resourceBuilder)
                 .AddAspNetCoreInstrumentation()
                 .AddRuntimeInstrumentation()
                 .AddProcessInstrumentation()
                 .AddEventCountersInstrumentation(options =>
                 {
                     options.AddEventSources("Microsoft.AspNetCore.Hosting");
                 })
                 .AddMeter($"{appName}.*")
                 .AddOtlpExporter(options =>
                 {
                     options.Endpoint = new Uri(otelUrl);
                 }));
        }
    }
}