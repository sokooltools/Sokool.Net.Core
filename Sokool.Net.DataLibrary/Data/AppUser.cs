using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Sokool.Net.DataLibrary.Data
{
	public class AppUser : IdentityUser
	{
		[Display(Name = "First Name")]
		[Required(ErrorMessage = "You need to provide your first name.")]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		[Required(ErrorMessage = "You need to provide your last name.")]
		public string LastName { get; set; }
	}
}
