using System.ComponentModel.DataAnnotations;

namespace Sokool.Net.Web.Models
{
	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
