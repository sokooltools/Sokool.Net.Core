using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sokool.Net.DataLibrary.Data;
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
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AccountController(
			ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
			)
		{
			_logger = logger;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Register()
		{
			Title = "User Sign Up";
			ViewBag.Message = "Sign-Up for full access to Sokool.net.";
			_logger.LogInformation(Title);
			return View(new RegisterViewModel());
		}

		[AcceptVerbs("Get","Post")]
		[AllowAnonymous]
		public async Task<IActionResult> IsEmailInUse(string email)
		{
			AppUser user = await _userManager.FindByEmailAsync(email);
			return user == null ? Json(true) : Json($"Email {email} is already in use");
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid) 
				return View(model);

			var user = new AppUser { 
				UserName = model.Email, 
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName
			};

			IdentityResult result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				// If the user is signed in and in the Admin role, then it is
				// the Admin user that is creating a new user. So redirect the
				// Admin user to ListRoles action
				if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
				{
					return RedirectToAction("ListUsers", "Administration");
				}

				await _signInManager.SignInAsync(user, isPersistent: false);
				return RedirectToAction("Index", "Home");
			}
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Login(string returnUrl)
		{
			Title = "User Login";
			ViewBag.Message = "Login for full access to Sokool.net.";
			return View(new LoginViewModel { ReturnUrl = returnUrl});
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid) 
				return View(model);

			SignInResult result = await _signInManager.PasswordSignInAsync(model.EmailAddress, model.Password, model.RememberMe, false);
			if (result.Succeeded)
			{
				return !String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
					? (IActionResult) Redirect(returnUrl)
					: RedirectToAction("Index", "Home");
			}
			ModelState.AddModelError(String.Empty, "Invalid Login Attempt");
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult AccessDenied()
		{
			Title = "Access Denied";
			return View();
		}
	}
}
