using Application.Services;
using Domain.Interfaces.Application;
using Domain.Interfaces.Infrastructure;
using Infrastructure.Brokers.Publishers;

namespace WebAPI.Extensions
{
	public static class ServicesExtensions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMessagePublisher, KafkaPublisher>();
            builder.Services.AddSingleton<IPaymentService, PaymentService>();
        }
    }
}
