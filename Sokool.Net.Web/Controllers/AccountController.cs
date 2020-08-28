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
		//------------------------------------------------------------------------------------------------------------------------
		// ReSharper disable once MemberCanBePrivate.Global
		public string Title { get; set; }

		private readonly ILogger<AccountController> _logger;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AccountController(
			ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
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

		[AcceptVerbs("Get", "Post")]
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
			var user = new AppUser
			{
				UserName = model.Email,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName
			};
			IdentityResult result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
			{
				// This next section used for email confirmation
				string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				string confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
				
				_logger.Log(LogLevel.Warning, confirmationLink);

				// If the user is signed in and in the Admin role, then it is the Admin user 
				// that is creating a new user. So redirect the Admin user to ListRoles action
				if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
				{
					return RedirectToAction("ListUsers", "Administration");
				}

				// TODO: send email containing confirmationLink

				//await _signInManager.SignInAsync(user, isPersistent: false);
				//return RedirectToAction("Index", "Home");

				ViewBag.ErrorTitle = "Registration successful";
				ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
									   "email, by clicking on the confirmation link we have emailed you...";
				return View("Exception");
			}
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View(model);
		}

		[AllowAnonymous]
		public async Task<IActionResult> ConfirmEmail(string userId, string token)
		{
			if (userId == null || token == null)
			{
				return RedirectToAction("index", "home");
			}
			AppUser user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
				return View("Exception");
			}
			IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
			if (result.Succeeded)
			{
				return View();
			}
			ViewBag.ErrorTitle = "Email cannot be confirmed";
			return View("Exception");
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Login(string returnUrl)
		{
			Title = "User Login";
			ViewBag.Message = "Login for full access to Sokool.net.";
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
				return View(model);

			// This next section is used to verify the user has confirmed registration via email response
			AppUser user = await _userManager.FindByEmailAsync(model.EmailAddress);
			if (user != null && !user.EmailConfirmed && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				ModelState.AddModelError(String.Empty, "Email not confirmed yet");
				return View(model);
			}

			//--------------------------------------------------------------------------------------------------------------------
			// The last boolean parameter lockoutOnFailure indicates if the account should be locked on failed logon attempt. 
			// On every failed logon attempt AccessFailedCount column value in AspNetUsers table is incremented by 1. When the 
			// AccessFailedCount reaches the configured MaxFailedAccessAttempts which in our case is 5, the account will be locked 
			// and LockoutEnd column is populated. After the account is lockedout, even if we provide the correct username and 
			// password, PasswordSignInAsync() method returns Lockedout result and the login will not be allowed for the duration 
			// the account is locked.
			//--------------------------------------------------------------------------------------------------------------------
			SignInResult result = await _signInManager.PasswordSignInAsync(model.EmailAddress, model.Password, model.RememberMe, true);
			if (result.Succeeded)
			{
				return !String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
					? (IActionResult)Redirect(returnUrl)
					: RedirectToAction("Index", "Home");
			}
			// If account is locked out, send the user to AccountLocked view.
			if (result.IsLockedOut)
			{
				return View("AccountLocked");
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

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgotPassword()
		{
			Title = "Forgot Password";
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			// Find the user by email
			AppUser user = await _userManager.FindByEmailAsync(model.Email);

			// If the user is found AND Email is not confirmed
			if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
				return View("ForgotPasswordConfirmation");

			// Generate the reset password token
			string token = await _userManager.GeneratePasswordResetTokenAsync(user);

			// Build the password reset link
			string passwordResetLink = Url.Action("ResetPassword", "Account",
				new { email = model.Email, token = token }, Request.Scheme);

			// Log the password reset link
			_logger.Log(LogLevel.Warning, passwordResetLink);

			// TODO: Send passwordResetLink via email

			// Send the user to Forgot Password Confirmation view.
			return View("ForgotPasswordConfirmation");

			// To avoid account enumeration and brute force attacks, don't
			// reveal that the user does not exist or is not confirmed.
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPassword(string token, string email)
		{
			Title = "Reset Password";
			// If password reset token or email is null, most likely the
			// user tried to tamper the password reset link
			if (token == null || email == null)
			{
				ModelState.AddModelError("", "Invalid password reset token");
			}
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			// Find the user by email
			AppUser user = await _userManager.FindByEmailAsync(model.Email);

			if (user == null)
				return View("ResetPasswordConfirmation");

			// Reset the user password
			IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
			if (result.Succeeded)
			{
				// Upon successful password reset and if the account is lockedout, set
				// the account lockout end date to current UTC date time, so the user
				// can login with the new password
				if (await _userManager.IsLockedOutAsync(user))
				{
					await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
				}
				return View("ResetPasswordConfirmation");
			}
			// To avoid account enumeration and brute force attacks, don't reveal that the
			// user does not exist.
			// Display validation errors. For example, password reset token already
			// used to change the password or password complexity rules not met.
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult ChangePassword()
		{
			Title = "Change Password";
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);
			AppUser user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return RedirectToAction("Login");
			}
			// ChangePasswordAsync changes the user password.
			IdentityResult result = await _userManager.ChangePasswordAsync(user,
				model.CurrentPassword, model.NewPassword);
			// The new password did not meet the complexity rules or the current password
			// is incorrect. Add these errors to he ModelState and rerender ChangePassword view.
			if (!result.Succeeded)
			{
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError(String.Empty, error.Description);
				}
				return View();
			}
			// Upon successfully changing the password refresh sign-in cookie
			await _signInManager.RefreshSignInAsync(user);
			return View("ChangePasswordConfirmation");
		}
	}
}
