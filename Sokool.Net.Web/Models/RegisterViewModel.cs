using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Sokool.Net.Web.Utilities;

namespace Sokool.Net.Web.Models
{
	public class RegisterViewModel
	{
		//[Display(Name = "User ID")]
		//[Range(1, 9999, ErrorMessage = "You need to enter a user id")]
		//public int UserId { get; set; }

		[Display(Name = "First Name")]
		[Required(ErrorMessage = "You need to provide your first name.")]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		[Required(ErrorMessage = "You need to provide your last name.")]
		public string LastName { get; set; }

		[Display(Name = "Email Address")]
		[Required(ErrorMessage = "You need to provide your email address.")]
		[EmailAddress]
		[Remote(action: "IsEmailInUse", controller: "Account")] // This uses jQuery validation
		//[ValidEmailDomain(allowedDomain: "sokool.net", ErrorMessage = "Email domain must be sokool.net")]  // <- this demonstrates custom validation
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		
		[Display(Name = "Confirm Email Addresss")]
		[Compare("Email", ErrorMessage = "The email and confirm email address do not match.")]
		public string ConfirmEmail { get; set; }
		
		[Display(Name = "Password")]
		[Required(ErrorMessage = "You must specify a password.")]
		[DataType(DataType.Password)]
		//[StringLength(32, MinimumLength = 8, ErrorMessage = "You need to provide at least an 8 character password.")]
		public string Password { get; set; }
		
		[Display(Name = "Confirm Password")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}