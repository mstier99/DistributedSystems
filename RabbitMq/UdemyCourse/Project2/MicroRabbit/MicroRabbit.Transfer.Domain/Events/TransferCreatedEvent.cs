using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Transfer.Domain.Events;
public class TransferCreatedEvent: Event
{
    public required int From { get; init; }
    public required int To { get; init; }
    public required decimal Amount { get; init; }
}
