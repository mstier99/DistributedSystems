using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Data.Repository;

public sealed class AccountRepository : IAccountRepository
{
    readonly BankingDbContext _bankingDbContext;

    public AccountRepository(BankingDbContext bankingDbContext)
    {
        _bankingDbContext = bankingDbContext;
    }

    public IEnumerable<Account> GetAccounts()
    {
        return _bankingDbContext.Accounts;
    }

    public void InsertAccount(CreateAccount account)
    {
        _bankingDbContext.Accounts.Add
        (
            new Account
            {
                AccountBalance = account.AccountBalance, 
                AccountType = account.AccountType
            }
        );
        _bankingDbContext.SaveChanges();
    }
}
