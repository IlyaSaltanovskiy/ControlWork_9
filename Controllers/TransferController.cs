using ControlWork_9.Managers;
using ControlWork_9.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Interfaces;

namespace ControlWork_9.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TransferController : ControllerBase
{
    private readonly ITransferService _transferService;
    private readonly ILogger<TransferController> _logger;

    public TransferController(ITransferService transferService, ILogger<TransferController> logger)
    {
        _transferService = transferService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> SendMoney([FromBody] TransferModelData tmd)
    {
        try
        {
            var userId = GetUserIdFromContext();
            if (userId.HasValue)
                tmd.SenderId = userId.Value;

            var result = await _transferService.SendMoney(tmd);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SendMoney.");
            return StatusCode(500, "An error occurred while processing the transaction.");
        }
    }

    [HttpGet]
    public IActionResult History()
    {
        try
        {
            var userId = GetUserIdFromContext();
            if (!userId.HasValue)
                return Unauthorized("Not authorized");

            var result = _transferService.GetTransferByUserId(userId.Value);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in History.");
            return StatusCode(500, "An error occurred while retrieving the transaction history.");
        }
    }

    [HttpGet]
    public IActionResult GetAllTransfers()
    {
        try
        {
            var list = _transferService.getAllTransfer();
            return Ok(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllTransfers.");
            return StatusCode(500, "An error occurred while retrieving all transfers.");
        }
    }

    private int? GetUserIdFromContext()
    {
        try
        {
            var idHash = HttpContext.GetUserIdHash();
            if (!string.IsNullOrWhiteSpace(idHash))
                return Convert.ToInt32(idHash);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve user ID from context.");
        }

        return null;
    }
}
