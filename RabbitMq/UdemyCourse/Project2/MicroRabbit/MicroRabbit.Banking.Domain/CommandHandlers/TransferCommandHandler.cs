using MicroRabbit.Banking.Domain.Commands;
using MediatR;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Domain.CommandHandlers;

public sealed class TransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
{
    readonly IEventBus _bus;

    public TransferCommandHandler(IEventBus bus)
    {
        _bus = bus;
    }

    public Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken = default)
    {
        var transferCreatedEvent = new TransferCreatedEvent
        {
            From = request.From,
            To = request.To,
            Amount = request.Amount
        };

        _bus.Publish(transferCreatedEvent);

        return Task.FromResult(true);
    }
}
