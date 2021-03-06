﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sokool.Net.DataLibrary.Data;
using Sokool.Net.Web.Models;

namespace Sokool.Net.Web.Controllers
{
	[Authorize(Roles = "SuperUser, Admin")]
	public class AdministrationController : Controller
	{
		[ViewData]
		// ReSharper disable once MemberCanBePrivate.Global
		public string Title { get; set; }

		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly ILogger<AdministrationController> _logger;

		public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, ILogger<AdministrationController> logger)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult ListUsers()
		{
			Title = "All Users";
			IQueryable<AppUser> users = _userManager.Users;
			return View(users);
		}

		[HttpGet]
		[Authorize(Policy = "EditUserPolicy")]
		public async Task<IActionResult> EditUser(string id)
		{
			Title = "Edit User";
			AppUser user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
				return View("Error");
			}

			// GetClaimsAsync returns the list of user Claims
			IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);

			// GetRolesAsync returns the list of user Roles
			IList<string> userRoles = await _userManager.GetRolesAsync(user);

			var model = new EditUserViewModel
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList(),
				Roles = userRoles
			};
			return View(model);
		}

		[HttpPost]
		[Authorize(Policy = "EditUserPolicy")]
		public async Task<IActionResult> EditUser(EditUserViewModel model)
		{
			AppUser user = await _userManager.FindByIdAsync(model.Id);
			if (user == null)
			{
				ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
				return View("Error");
			}
			user.Email = model.Email;
			user.UserName = model.UserName;
			user.FirstName = model.FirstName;
			user.LastName = model.LastName;

			IdentityResult result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				return RedirectToAction("ListUsers");
			}
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError(String.Empty, error.Description);
			}
			return View(model);
		}

		[HttpPost]
		[Authorize(Policy = "DeleteUserPolicy")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			AppUser user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
				return View("Error");
			}
			IdentityResult result = await _userManager.DeleteAsync(user);
			if (result.Succeeded)
			{
				return RedirectToAction("ListUsers");
			}
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError(String.Empty, error.Description);
			}
			return View("ListUsers");
		}

		// ------------------------------------------------------------------------------------------------------

		[HttpGet]
		public IActionResult ListRoles()
		{
			Title = "All Roles";
			IQueryable<IdentityRole> roles = _roleManager.Roles;
			return View(roles);
		}

		[HttpGet]
		[Authorize(Policy = "CreateRolePolicy")]
		public IActionResult CreateRole()
		{
			Title = "Create Role";
			return View();
		}

		[HttpPost]
		[Authorize(Policy = "CreateRolePolicy")]
		public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);
			var identityRole = new IdentityRole
			{
				Name = model.RoleName
			};
			IdentityResult result = await _roleManager.CreateAsync(identityRole);
			if (result.Succeeded)
			{
				return RedirectToAction("ListRoles", "Administration");
			}
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError(String.Empty, error.Description);
			}
			return View(model);
		}

		[HttpGet]
		[Authorize(Policy = "EditRolePolicy")]
		public async Task<IActionResult> EditRole(string id)
		{
			Title = "Edit Role";
			IdentityRole role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				ViewBag.ErrorMessage = $"Role with ID = {id} cannot be found";
				return View("Error");
			}
			var model = new EditRoleViewModel
			{
				Id = role.Id,
				RoleName = role.Name
			};
			foreach (AppUser user in _userManager.Users)
			{
				if (await _userManager.IsInRoleAsync(user, role.Name))
				{
					model.Users.Add(user.UserName);
				}
			}
			return View(model);
		}

		[HttpPost]
		[Authorize(Policy = "EditRolePolicy")]
		public async Task<IActionResult> EditRole(EditRoleViewModel model)
		{
			IdentityRole role = await _roleManager.FindByIdAsync(model.Id);
			if (role == null)
			{
				ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
				return View("Error");
			}
			role.Name = model.RoleName;
			IdentityResult result = await _roleManager.UpdateAsync(role);
			if (result.Succeeded)
			{
				return RedirectToAction("ListRoles");
			}
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError(String.Empty, error.Description);
			}
			return View(model);
		}

		[HttpGet]
		[Authorize(Policy = "EditUsersInRolePolicy")]
		public async Task<IActionResult> EditUsersInRole(string roleId)
		{
			Title = "Edit Users In Role";

			ViewBag.roleId = roleId;

			IdentityRole role = await _roleManager.FindByIdAsync(roleId);
			if (role == null)
			{
				ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
				return View("Error");
			}
			var model = new List<UsersInRoleViewModel>();
			foreach (AppUser user in _userManager.Users)
			{
				var usersInRoleViewModel = new UsersInRoleViewModel
				{
					UserId = user.Id,
					UserName = user.UserName,
					IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
				};
				model.Add(usersInRoleViewModel);
			}
			return View(model);
		}

		[HttpPost]
		[Authorize(Policy = "EditUsersInRolePolicy")]
		public async Task<IActionResult> EditUsersInRole(List<UsersInRoleViewModel> model, string roleId)
		{
			IdentityRole role = await _roleManager.FindByIdAsync(roleId);
			if (role == null)
			{
				ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
				return View("Error");
			}
			for (int i = 0; i < model.Count; i++)
			{
				AppUser user = await _userManager.FindByIdAsync(model[i].UserId);

				IdentityResult result;

				bool isInRole = await _userManager.IsInRoleAsync(user, role.Name);

				switch (model[i].IsSelected)
				{
					case true when !isInRole:
						result = await _userManager.AddToRoleAsync(user, role.Name);
						break;
					case false when isInRole:
						result = await _userManager.RemoveFromRoleAsync(user, role.Name);
						break;
					default:
						continue;
				}
				if (!result.Succeeded)
					continue;

				if (i >= model.Count - 1)
					break;
			}
			return RedirectToAction("EditRole", new { Id = roleId });
		}

		[HttpPost]
		[Authorize(Policy = "DeleteRolePolicy")]
		public async Task<IActionResult> DeleteRole(string id)
		{
			IdentityRole role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
				return View("Error");
			}
			try
			{
				IdentityResult result = await _roleManager.DeleteAsync(role);
				if (result.Succeeded)
				{
					return RedirectToAction("ListRoles");
				}
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError(String.Empty, error.Description);
				}
				return View("ListRoles");
			}
			catch (DbUpdateException ex)
			{
				// Log the exception to a file.
				_logger.LogError($"Exception occured : {ex}");
				// Pass the ErrorTitle and ErrorMessage that you want to show to the user using ViewBag. The Error view retrieves this data
				// from the ViewBag and displays to the user.
				ViewBag.ErrorTitle = $"The '{role.Name}' role is currently in use";
				ViewBag.ErrorMessage = "The role cannot be deleted since it has been assigned to one or more users. " +
									   "To delete this role, requires that you must first remove all users assigned to it.";
				return View("Exception");
			}
		}

		[HttpGet]
		[Authorize(Policy = "ManageUserRolesPolicy")]
		public async Task<IActionResult> ManageUserRoles(string userId)
		{
			Title = "Manage User Roles";
			ViewBag.userId = userId;
			AppUser user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
				return View("Error");
			}
			var model = new List<UserRolesViewModel>();
			foreach (IdentityRole role in _roleManager.Roles)
			{
				var userRolesViewModel = new UserRolesViewModel
				{
					RoleId = role.Id,
					RoleName = role.Name,
					IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
				};
				model.Add(userRolesViewModel);
			}
			return View(model);
		}

		[HttpPost]
		[Authorize(Policy = "ManageUserRolesPolicy")]
		public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
		{
			AppUser user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
				return View("Error");
			}
			IList<string> roles = await _userManager.GetRolesAsync(user);
			IdentityResult result = await _userManager.RemoveFromRolesAsync(user, roles);
			if (!result.Succeeded)
			{
				ModelState.AddModelError(String.Empty, "Cannot remove user existing roles");
				return View(model);
			}
			result = await _userManager.AddToRolesAsync(user,
				model.Where(x => x.IsSelected).Select(y => y.RoleName));
			if (result.Succeeded)
				return RedirectToAction("EditUser", new { Id = userId });
			ModelState.AddModelError(String.Empty, "Cannot add selected roles to user");
			return View(model);
		}

		[HttpGet]
		[Authorize(Policy = "ManageUserClaimsPolicy")]
		public async Task<IActionResult> ManageUserClaims(string userId)
		{
			Title = "Manage User Claims";
			AppUser user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
				return View("Error");
			}
			// _userManager service GetClaimsAsync method gets all the current claims of the user
			IList<Claim> existingUserClaims = await _userManager.GetClaimsAsync(user);
			var model = new UserClaimsViewModel
			{
				UserId = userId
			};
			// Loop through each claim we have in our application
			foreach (Claim claim in ClaimsStore.AllClaims)
			{
				var userClaim = new UserClaim
				{
					ClaimType = claim.Type
				};
				// If the user has the claim, set IsSelected property to true, so the checkbox
				// next to the claim is checked on the UI
				if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
				{
					userClaim.IsSelected = true;
				}
				model.Claims.Add(userClaim);
			}
			return View(model);
		}

		[HttpPost]
		[Authorize(Policy = "ManageUserClaimsPolicy")]
		public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
		{
			AppUser user = await _userManager.FindByIdAsync(model.UserId);
			if (user == null)
			{
				ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
				return View("Error");
			}
			// Get all the user existing claims and delete them
			IList<Claim> claims = await _userManager.GetClaimsAsync(user);
			IdentityResult result = await _userManager.RemoveClaimsAsync(user, claims);
			if (!result.Succeeded)
			{
				ModelState.AddModelError(String.Empty, "Cannot remove user existing claims");
				return View(model);
			}
			// Add all the claims that are selected on the UI
			result = await _userManager.AddClaimsAsync(user,
				model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));
			if (result.Succeeded)
				return RedirectToAction("EditUser", new { Id = model.UserId });

			ModelState.AddModelError(String.Empty, "Cannot add selected claims to user");
			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult AccessDenied()
		{
			return View("~/Views/Account/AccessDenied.cshtml");
		}
	}
}
