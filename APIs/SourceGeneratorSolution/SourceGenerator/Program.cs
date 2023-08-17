
using SourceGenerator.Services;
using System.Text.Json;

namespace SourceGenerator;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors
        (
            options => options.AddDefaultPolicy
            (
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            )
        );

        builder.Services.AddSingleton<MessageGenerator>();

        builder.Services.AddHttpClient<MessageClient>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        app.Run();
    }
}