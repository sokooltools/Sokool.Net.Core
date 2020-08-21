using System.ComponentModel.DataAnnotations;

namespace Sokool.Net.Web.Models
{
	public class CreateRoleViewModel
	{
		[Required]
		[Display(Name = "Role Name")]
		public string RoleName { get; set; }
	}
}
