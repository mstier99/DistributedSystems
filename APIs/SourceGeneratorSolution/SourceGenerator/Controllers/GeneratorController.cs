using Microsoft.AspNetCore.Mvc;
using SourceGenerator.Services;

namespace SourceGenerator.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class GeneratorController:ControllerBase
{
    readonly MessageGenerator _generator;
    readonly MessageClient _messageClient;

    public GeneratorController(MessageGenerator generator, MessageClient messageClient)
	{
        _generator = generator;
        _messageClient = messageClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetGeneratedMessages(int count)
    {
        var messages = _generator.GenerateMessages(count);

        return await Task.FromResult(Ok(messages));
    }

    [HttpGet]
    public async Task<IActionResult> Send()
    {
        var result = await _messageClient.TestAsync();

        return Ok(result);
    }
}
