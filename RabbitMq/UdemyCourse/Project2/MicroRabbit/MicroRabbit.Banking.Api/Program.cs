using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BankingDbContext>
(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString(nameof(BankingDbContext))
        )
);

// Register services from another projects
DependencyContainer.RegisterServices(
    builder.Services, 
    registerMediatRAssembly: typeof(MicroRabbit.Infra.Bus.RabbitMQBus).Assembly
); 

// Pipeline

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();