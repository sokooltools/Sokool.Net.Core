using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sokool.Net.Web.Controllers
{
	public class ErrorController : Controller
	{
		private readonly ILogger<ErrorController> _logger;
		private readonly IWebHostEnvironment _env;

		public ErrorController(ILogger<ErrorController> logger, IWebHostEnvironment env)
		{
			_env = env;
			_logger = logger;
		}

		[Route("Error/{statusCode}")]
		public IActionResult HttpStatusCodeHandler(int statusCode)
		{
			var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
			ViewBag.Path = statusCodeResult.OriginalPath;
			ViewBag.QS = statusCodeResult.OriginalQueryString;

			(ViewBag.StatusCode, ViewBag.ErrorType, ViewBag.ErrorMessage) = ExceptionHandler.GetDesc(statusCode);

			return View("Error");
		}

		[Route("Error")]
		[AllowAnonymous]
		public IActionResult Error()
		{
			// Log the following:
			var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
			ViewBag.ExceptionPath = exceptionDetails.Path;
			ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
			ViewBag.Stacktrace = exceptionDetails.Error.StackTrace;

			_logger.LogError($"The path {exceptionDetails.Path} threw an exception {exceptionDetails.Error.Message}");

			return View("Exception");
		}

		//[AllowAnonymous]
		//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		//public IActionResult Error()
		//{
		//	return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
