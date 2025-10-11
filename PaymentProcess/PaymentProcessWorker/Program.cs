using Application.UseCases;
using Domain.Interfaces;
using Infrastructure.Brokers.Consumers;
using PaymentProcessWorker.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.AddConfigurations();
builder.AddLog();
builder.AddOtel();

builder.Services.AddHostedService<PaymentCreatedConsumer>();

builder.Services.AddSingleton<IPaymentCreatedUseCase, PaymentCreatedUseCase>();

var host = builder.Build();
host.Run();
