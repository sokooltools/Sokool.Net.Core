using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sokool.Net.DataLibrary.BusinessLogic;
using Sokool.Net.Web.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Sokool.Net.Web.Controllers
{
	public class AccountController : Controller
	{
		[ViewData]
		// ReSharper disable once MemberCanBePrivate.Global
		public string Title { get; set; }

		private readonly ILogger<AccountController> _logger;
		//private readonly UserManager<IdentityUser> _userManager;
		//private readonly SignInManager<IdentityUser> _signInManager;

		public AccountController(
			ILogger<AccountController> logger
			//,UserManager<IdentityUser> userManager,
			//SignInManager<IdentityUser> signInManager
			)
		{
			_logger = logger;
			//_userManager = userManager;
			//_signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Register()
		{
			Title = "User Sign Up";
			ViewBag.Message = "Sign-Up for full access to Sokool.net.";
			_logger.LogInformation(Title);
			return View(new RegisterViewModel());
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public IActionResult Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("MyError", "You have entered some invalid data. (See below)");
				return View();
			}
			UserProcessor.CreateUser(model.UserId, model.FirstName, model.LastName, model.EmailAddress);
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult Login()
		{
			Title = "User Login";
			ViewBag.Message = "Login for full access to Sokool.net.";
			return View(new LoginViewModel());
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				//SignInResult result = await _signInManager.PasswordSignInAsync(model.EmailAddress, model.Password, model.RememberMe, false);
				//if (result.Succeeded)
				//{
				//	return RedirectToAction("Index", "Home");
				//}
				ModelState.AddModelError(String.Empty, "Invalid Login Attempt");
			}
			//ModelState.AddModelError("MyError", "You have entered some invalid data. (See below)");
			//return RedirectToAction("Index", "Home");
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			//await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
