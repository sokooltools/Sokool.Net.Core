using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sokool.Net.DataLibrary.Data;
using Sokool.Net.Web.Models;

namespace Sokool.Net.Web.Controllers
{
	[Authorize(Roles="Admin")]
	public class AdministrationController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<AppUser> _userManager;

		public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult ListUsers()
		{
			IQueryable<AppUser> users = _userManager.Users;
			return View(users);
		}

		[HttpGet]
		public IActionResult CreateRole()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> EditRole(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
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

		[HttpGet]
		public IActionResult DeleteRole()
		{
			return null;
		}

		[HttpPost]
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
				ModelState.AddModelError("", error.Description);
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult ListRoles()
		{
			IQueryable<IdentityRole> roles = _roleManager.Roles;
			return View(roles);
		}

		[HttpPost]
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
		public async Task<IActionResult> EditUsersInRole(string roleId)
		{
			ViewBag.roleId = roleId;

			IdentityRole role = await _roleManager.FindByIdAsync(roleId);
			if (role == null)
			{
				ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
				return View("Error");
			}

			var model = new List<UserRoleViewModel>();
			foreach (AppUser user in _userManager.Users)
			{
				var userRoleViewModel = new UserRoleViewModel
				{
					UserId = user.Id,
					UserName = user.UserName
				};
				userRoleViewModel.IsSelected = await _userManager.IsInRoleAsync(user, role.Name);
				model.Add(userRoleViewModel);
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
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

				if (model[i].IsSelected && !isInRole)
				{
					result = await _userManager.AddToRoleAsync(user, role.Name);
				}
				else if (!model[i].IsSelected && isInRole)
				{
					result = await _userManager.RemoveFromRoleAsync(user, role.Name);
				}
				else
				{
					continue;
				}
				if (!result.Succeeded)
					continue;

				if (i >= model.Count - 1)
					break;

			}
			return RedirectToAction("EditRole", new { Id = roleId });
		}
	}
}
