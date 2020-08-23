using System.Collections.Generic;
using Sokool.Net.DataLibrary.Data;

namespace Sokool.Net.Web.Models
{
	public class UserClaimsViewModel
	{
		public UserClaimsViewModel()
		{
			Claims = new List<UserClaim>();
		}

		public string UserId { get; set; }

		public List<UserClaim> Claims { get; set; }
	}
}
