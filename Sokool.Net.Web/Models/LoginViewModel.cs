using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Sokool.Net.Web.Models
{
	public class LoginViewModel
	{
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email Address")]
		[Required(ErrorMessage = "You need to provide your email address.")]
		public string EmailAddress { get; set; }
		
		[Display(Name = "Password")]
		[Required(ErrorMessage = "You need to provide your password.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		
		[Display(Name = "Remember me")]
		public bool RememberMe { get; set; }

		[HiddenInput(DisplayValue = false)]
		public string ReturnUrl { get; set; }
	}
}