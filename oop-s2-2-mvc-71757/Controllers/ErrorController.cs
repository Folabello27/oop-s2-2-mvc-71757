using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace oop_s2_2_mvc_71757.Controllers;

public class ErrorController : Controller
{
    private readonly Serilog.ILogger _logger = Log.ForContext<ErrorController>();

    [Route("Error")]
    public IActionResult Error()
    {
        var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandler?.Error is not null)
        {
            _logger.Error(exceptionHandler.Error, "Unhandled exception on {Path}", exceptionHandler.Path);
        }

        return View("ErrorFriendly");
    }
}
