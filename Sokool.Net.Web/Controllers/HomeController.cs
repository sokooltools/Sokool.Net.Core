using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;
using Microsoft.Extensions.Logging;
using Sokool.Net.DataLibrary.BusinessLogic;
using Sokool.Net.Web.Models;

namespace Sokool.Net.Web.Controllers
{
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

		public IActionResult Index()
		{
			Title = "Index";
			_logger.LogInformation("'Index'");
			return View();
		}

		public IActionResult Privacy()
		{
			Title = "Privacy";
			ViewBag.Message = "Privacy Policy";

			_logger.LogInformation("'Privacy'");

			return View();
		}

		//[CustomActionFilter("About")]
		[HttpGet]
		public IActionResult About()
		{
			Title = "About";
			ViewBag.Message = "About the Sokool.net web-site";
			return View();
		}

		[HttpGet]
		public IActionResult Contact()
		{
			Title = "Contact";
			ViewBag.Message = "How to contact Sokool.net web-site support.";
			return View();
		}

		[HttpGet]
		public ActionResult SignUp()
		{
			Title = "User Sign Up";
			ViewBag.Message = "Sign-Up for full access to Sokool.net.";
			return View(new UserModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SignUp(UserModel model)
		{
			if (!ModelState.IsValid)
				return View();

			UserProcessor.CreateUser(model.UserId, model.FirstName, model.LastName, model.EmailAddress);
			return RedirectToAction("Index");
		}

		[AllowAnonymous]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
