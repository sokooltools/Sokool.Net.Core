using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Sokool.Net.DataLibrary.Data;

namespace Sokool.Net.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			const string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SokoolNet;Integrated Security=True;";
			services.AddDbContext<UserContext>(options => options.UseSqlServer(conn, b => b.MigrationsAssembly("Sokool.Net.DataLibrary")));

			services.AddControllersWithViews();

#if DEBUG
			// Add nuget package: Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation before adding this line:
			services.AddRazorPages().AddRazorRuntimeCompilation();
#endif
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else  // See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1
			{
				//app.UseExceptionHandler("/Home/Error");
				app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
			}

			app.UseStaticFiles();

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Assets")),
				RequestPath = new PathString("/assets")
			});

			if (env.IsDevelopment())
			{
				app.UseDirectoryBrowser(new DirectoryBrowserOptions
				{
					FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Assets")),
					RequestPath = "/Assets"
				});
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				//endpoints.MapControllerRoute(
				//	name: "default",
				//	pattern: "{controller=Home}/{action=Index}/{id?}");

				// This is a shortened version of preceding code.
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}
