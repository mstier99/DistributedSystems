using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Banking.Application.Services;

public sealed class AccountService : IAccountService
{
    readonly IAccountRepository _accountRepository;
    readonly IEventBus _bus;

    public AccountService(IAccountRepository accountRepository, IEventBus bus)
    {
        _accountRepository = accountRepository;
        _bus = bus;
    }

    public IEnumerable<Account> GetAccounts()
    {
        return _accountRepository.GetAccounts();
    }

    public void InsertAccount(CreateAccount account)
    {
        _accountRepository.InsertAccount(account);
    }

    public void Transfer(AccountTransfer accountTransfer)
    {
        var createTransferCommand = new CreateTransferCommand(
             from   : accountTransfer.From,
             to     : accountTransfer.To,
             amount : accountTransfer.Amount
        );

        _bus.SendCommand(createTransferCommand);
    }
}