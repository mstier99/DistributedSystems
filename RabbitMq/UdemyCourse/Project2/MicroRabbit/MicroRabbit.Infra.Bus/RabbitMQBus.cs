using System.Text;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroRabbit.Infra.Bus;

public sealed class RabbitMQBus : IEventBus
{
    readonly IMediator _mediator;
    readonly Dictionary<string, List<Type>> _handlers;
    readonly List<Type> _evenTypes;
    readonly IServiceScopeFactory _serviceScopeFactory;

    public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
    {
        _mediator = mediator;
        _serviceScopeFactory = serviceScopeFactory;
        _handlers = new Dictionary<string, List<Type>>();
        _evenTypes = new List<Type>();
    }


    public Task SendCommand<T>(T command) where T : Command
    {
        return _mediator.Send(command);
    }

    public void Publish<T>(T @event) where T : Event
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "myuser",
            Password = "mypassword"
        };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var eventName = @event.GetType().Name;

        channel.QueueDeclare(eventName, false, false, false, null);

        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("", eventName, null, body);
    }

    public void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);

        if (!_evenTypes.Contains(typeof(T)))
        {
            _evenTypes.Add(typeof(T));
        }

        if (!_handlers.ContainsKey(eventName))
        {
            _handlers.Add(eventName, new List<Type>());
        }

        if (_handlers[eventName].Any(s => s.GetType() == handlerType))
        {
            throw new ArgumentException
            (
                $"Handler type {handlerType.Name} name already registered for {eventName}.",
                nameof(handlerType)
            );
        }

        _handlers[eventName].Add(handlerType);

        StartBasicConsume<T>();
    }

    void StartBasicConsume<T>() where T : Event
    {
        var factory = new ConnectionFactory()
        {
            // conncet
            HostName = "localhost",
            Port = 5672,
            UserName = "myuser",
            Password = "mypassword",

            // behavior
            DispatchConsumersAsync = true
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var eventName = typeof(T).Name;

        channel.QueueDeclare(eventName, false, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += Consumer_Received;

        channel.BasicConsume(eventName, true, consumer);
    }

    async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;
        var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    async Task ProcessEvent(string eventName, string message)
    {
        if (!_handlers.ContainsKey(eventName))
            return;

        var subscriptions = _handlers[eventName];

        if (!subscriptions.Any())
            return;

        using var scope = _serviceScopeFactory.CreateScope();

        foreach (Type subscription in subscriptions)
        {
            var handler = scope.ServiceProvider.GetRequiredService(subscription);

            if (handler is null) continue;

            var eventType = _evenTypes.SingleOrDefault(eventType => eventType.Name == eventName);

            var @event = JsonConvert.DeserializeObject(message, eventType);

            var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new[] { @event });
        }
    }
}
