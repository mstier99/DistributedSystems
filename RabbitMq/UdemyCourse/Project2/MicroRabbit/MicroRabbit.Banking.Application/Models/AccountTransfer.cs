namespace MicroRabbit.Banking.Application.Models;

public class AccountTransfer
{
    public required int From { get; init; }

    public required int To { get; init; }

    public required decimal Amount { get; init; }
}