using System.ComponentModel.DataAnnotations;

namespace Sokool.Net.Web.Models
{
	public class RegisterViewModel
	{
		[Display(Name = "User ID")]
		[Range(1000, 9999, ErrorMessage = "You need to enter a four digit user id")]
		public int UserId { get; set; }
		
		[Display(Name = "First Name")]
		[Required(ErrorMessage = "You need to provide your first name.")]
		public string FirstName { get; set; }
		
		[Display(Name = "Last Name")]
		[Required(ErrorMessage = "You need to provide your last name.")]
		public string LastName { get; set; }
		
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email Address")]
		[Required(ErrorMessage = "You need to provide your email address.")]
		public string EmailAddress { get; set; }
		
		[Compare("EmailAddress", ErrorMessage = "THe Email and Confirm Email must match.")]
		[Display(Name = "Confirm Email")]
		public string ConfirmEmail { get; set; }
		
		[Display(Name = "Password")]
		[Required(ErrorMessage = "You must have a password.")]
		[DataType(DataType.Password)]
		[StringLength(100, MinimumLength = 8, ErrorMessage = "You need to provide at least an 8 character password.")]
		public string Password { get; set; }
		
		[Display(Name = "Confirm Password")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirm password must match.")]
		public string ConfirmPassword { get; set; }
	}
}