using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;

namespace MicroRabbit.Infra.IoC;

public static class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services, Assembly registerMediatRAssembly)
    {
        // Domain Bus
        services.AddSingleton<IEventBus, RabbitMQBus>(serviceProvider =>
        {
            var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            return new RabbitMQBus(serviceProvider.GetService<IMediator>(), serviceScopeFactory);
        });

        // Subscriptions
        services.AddTransient<TransferEventHandler>();

        // Domain Events
        services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

        // Application Services
        services.AddTransient<IAccountService, AccountService>();

        services.AddTransient<ITransferService, TransferService>();

        // Microservices Data
        services.AddTransient<BankingDbContext>();
        services.AddTransient<IAccountRepository, AccountRepository>();

        services.AddTransient<TransferDbContext>();
        services.AddTransient<ITransferRepository, TransferRepository>();

        // MediatR
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(registerMediatRAssembly));

        // Domain Commands
        services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();
    }
}
