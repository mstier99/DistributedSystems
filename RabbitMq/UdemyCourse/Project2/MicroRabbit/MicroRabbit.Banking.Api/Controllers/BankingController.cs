using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers;

[ApiController]
[Route("Api/[controller]/[action]")]
public sealed class BankingController : ControllerBase
{
    readonly IAccountService _accountService;

    public BankingController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts()
    {
        var result = _accountService.GetAccounts();

        return await Task.FromResult(
            Ok(result)
            );
    }

    [HttpPost]
    public async Task<IActionResult> InsertAccount([FromBody] CreateAccount account)
    {
        _accountService.InsertAccount(account);

        return await Task.FromResult(
            Ok()
            );
    }

    [HttpPost]
    public async Task<IActionResult> PostAccountTransfer([FromBody] AccountTransfer accountTransfer)
    {
        _accountService.Transfer(accountTransfer);
        
        return await Task.FromResult(
            Ok()
            );
    }
}
