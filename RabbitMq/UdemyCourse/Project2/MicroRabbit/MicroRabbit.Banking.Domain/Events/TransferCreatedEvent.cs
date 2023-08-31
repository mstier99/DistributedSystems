using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Banking.Domain.Events;

public class TransferCreatedEvent: Event
{
    public required int From { get; set; }

    public required int To { get; set; }

    public required decimal Amount { get; set; }
}
