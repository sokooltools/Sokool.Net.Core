using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Sokool.Net.DataLibrary.Data
{
	public class UserContext : DbContext
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options)
		{
			
		}

		public DbSet<User> User { get; set; }

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlServer("Server=.;Database=SokoolNet;Integrated Security=True;");
		//	base.OnConfiguring(optionsBuilder);
		//}
	}
}
