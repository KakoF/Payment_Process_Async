using Application.Services;
using Domain.Interfaces.Application;
using Domain.Interfaces.Infrastructure.Brokers;
using Domain.Interfaces.Infrastructure.Idempotency;
using Domain.Interfaces.Infrastructure.Redis;
using Domain.Records.CreatePayment;
using FluentValidation;
using Infrastructure.Brokers.Publishers;
using Infrastructure.Idempotency;
using Infrastructure.Meters;
using Infrastructure.Redis;
using WebAPI.Filters;

namespace WebAPI.Extensions
{
	public static class ServicesExtensions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMessagePublisher, KafkaPublisher>();
            builder.Services.AddSingleton<IPaymentService, PaymentService>();
			builder.Services.AddSingleton<MetricsHelper>();
			builder.Services.AddScoped<IRedis, Redis>();
			builder.Services.AddScoped<IIdempotencyStorage, IdempotencyStorage>();
			builder.Services.AddScoped<IdempotencyFilter>();
			builder.Services.AddScoped(typeof(ValidationFilter<>));
			builder.Services.AddScoped<IValidator<CreatePaymentRequest>, CreatePaymentValidator>();


		}
	}
}
