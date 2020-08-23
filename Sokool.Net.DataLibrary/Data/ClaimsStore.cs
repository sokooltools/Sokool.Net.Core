using System.Collections.Generic;
using System.Security.Claims;

namespace Sokool.Net.DataLibrary.Data
{
	public static class ClaimsStore
	{
		public static List<Claim> AllClaims = new List<Claim>
		{
			new Claim("CreateUser", "CreateUser"),
			new Claim("EditUser","EditUser"),
			new Claim("DeleteUser","DeleteUser"),
			new Claim("EditUsersInRole","EditUsersInRole"),
			new Claim("ManageUserRoles","ManageUserRoles"),
			new Claim("ManageUserClaims","ManageUserClaims"),
			
			new Claim("CreateRole", "CreateRole"),
			new Claim("EditRole","EditRole"),
			new Claim("DeleteRole","DeleteRole")
		};
	}
}
