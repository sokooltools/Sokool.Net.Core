using System;
using System.ComponentModel.DataAnnotations;

namespace Sokool.Net.DataLibrary.Data
{
	public class User
	{
		[Display(Name = "User ID")]
		[Range(100000, 999999, ErrorMessage = "You need to enter a valid User id")]
		public int UserId { get; set; }
		
		[Display(Name = "First Name")]
		[Required(ErrorMessage = "You need to give us your first name.")]
		public string FirstName { get; set; }
		
		[Display(Name = "Last Name")]
		[Required(ErrorMessage = "You need to give us your last name.")]
		public string LastName { get; set; }
		
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email Address")]
		[Required(ErrorMessage = "You need to give us your email address.")]
		public string EmailAddress { get; set; }

		public DateTime CreatedDateTime { get; set; }
	}
}
