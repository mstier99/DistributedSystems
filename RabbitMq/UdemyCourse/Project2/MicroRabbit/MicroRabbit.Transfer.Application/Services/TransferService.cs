using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Application.Services;

public class TransferService : ITransferService
{
    readonly IEventBus _bus;
    readonly ITransferRepository _transferRepository;

    public TransferService(IEventBus bus)
    {
        _bus = bus;
    }

    public IEnumerable<TransferLog> GeTransferLogs()
    {
        return _transferRepository.GetTransferLogs();
    }
}
