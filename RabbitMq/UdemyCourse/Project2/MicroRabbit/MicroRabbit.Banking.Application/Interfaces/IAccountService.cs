﻿using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Application.Interfaces;

public interface IAccountService
{
    IEnumerable<Account> GetAccounts();

    void InsertAccount(CreateAccount account);

    void Transfer(AccountTransfer accountTransfer);
}