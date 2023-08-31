namespace MicroRabbit.Domain.Core.Events;

public abstract class Event
{
    public DateTime TimeStamp { get; } = DateTime.Now;
}
