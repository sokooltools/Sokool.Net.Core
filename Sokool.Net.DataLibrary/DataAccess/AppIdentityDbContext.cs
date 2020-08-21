using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sokool.Net.DataLibrary.Data;

namespace Sokool.Net.DataLibrary.DataAccess
{
	public class AppIdentityDbContext : IdentityDbContext<AppUser>
	{
		public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options): base(options) { }

		//public DbSet<User> Users { get; set; }

		//protected override void OnModelCreating(ModelBuilder builder)
		//{
		//	base.OnModelCreating(builder);
		//	builder.Seed();
		//}
	}
}