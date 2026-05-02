using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProcessController : ControllerBase
{
  public static ILogger<ProcessController>? _logger { get; set; }
  public static IProcessService? _processService { get; set; }

  public ProcessController(ILogger<ProcessController> logger, IProcessService processService)
  {
    _logger = logger;
    _processService = processService;
  }

  [HttpPost("ProcessHURDAT2")]
  public async Task<IActionResult> ProcessHURDAT2(IFormFile file)
  {
    List<StormInfo> stormInfoList = _processService!.ProcessHURDAT2(file);
    return Ok(stormInfoList);
  }
}