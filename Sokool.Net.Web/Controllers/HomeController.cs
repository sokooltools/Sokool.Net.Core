using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;
using Microsoft.Extensions.Logging;
using Sokool.Net.Web.Models;

namespace Sokool.Net.Web.Controllers
{
	//[Route("[controller]/[action]")]
	public class HomeController : Controller
	{
		[ViewData]
		// ReSharper disable once MemberCanBePrivate.Global
		public string Title { get; set; }
		
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		//[Route("")]
		//[Route("~/")]
		public IActionResult Index()
		{
			Title = "Index";
			_logger.LogInformation(ControllerContext.ToCtxString());
			return View();
		}

		public IActionResult Privacy()
		{
			Title = "Privacy";
			_logger.LogInformation(ControllerContext.ToCtxString());
			ViewBag.Message = "Privacy Policy";

			return View();
		}

		//[CustomActionFilter("About")]
		[HttpGet]
		public IActionResult About()
		{
			Title = "About";
			_logger.LogInformation(ControllerContext.ToCtxString());
			ViewBag.Message = "About the Sokool.net web-site";
			return View();
		}

		[HttpGet]
		public IActionResult Contact()
		{
			Title = "Contact";
			_logger.LogInformation(ControllerContext.ToCtxString());
			ViewBag.Message = "How to contact Sokool.net web-site support.";
			return View();
		}
	}
}
