using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sokool.Net.DataLibrary.Data;

namespace Sokool.Net.DataLibrary.DataAccess
{
	public class AppIdentityDbContext : IdentityDbContext<AppUser>
	{
		public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options): base(options) { }
	}
}
