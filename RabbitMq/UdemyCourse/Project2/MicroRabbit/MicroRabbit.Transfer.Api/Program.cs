using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.IoC;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<TransferDbContext>
(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString(nameof(TransferDbContext))
    )
);

// Register services from another projects
DependencyContainer.RegisterServices(
    builder.Services,
    registerMediatRAssembly: typeof(MicroRabbit.Infra.Bus.RabbitMQBus).Assembly
);

var app = builder.Build();

// Pipeline

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ConfigureEventBus(app);

app.Run();


void ConfigureEventBus(WebApplication app)
{
    var eventBus = app.Services.GetRequiredService<IEventBus>();
    eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
}

