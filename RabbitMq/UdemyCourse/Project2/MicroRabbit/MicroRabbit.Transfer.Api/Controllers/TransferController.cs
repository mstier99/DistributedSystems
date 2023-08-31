using MicroRabbit.Transfer.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Transfer.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]/[action]")]
    public class TransferController : ControllerBase
    {
        readonly ITransferRepository _transferRepository;

        public TransferController(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransfers()
        {
            var result = _transferRepository.GetTransferLogs();

            return await Task.FromResult(
                Ok(result)
                );
        }
    }
}