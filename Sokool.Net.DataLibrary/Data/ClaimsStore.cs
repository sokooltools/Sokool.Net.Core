using System.Collections.Generic;
using System.Security.Claims;

namespace Sokool.Net.DataLibrary.Data
{
	public static class ClaimsStore
	{
		public static List<Claim> AllClaims = new List<Claim>
		{
			new Claim("CreateUser", "true"),
			new Claim("EditUser","true"),
			new Claim("DeleteUser","true"),
			new Claim("EditUsersInRole","true"),
			new Claim("ManageUserRoles","true"),
			new Claim("ManageUserClaims","true"),
			
			new Claim("CreateRole", "true"),
			new Claim("EditRole","true"),
			new Claim("DeleteRole","true")
		};
	}
}
