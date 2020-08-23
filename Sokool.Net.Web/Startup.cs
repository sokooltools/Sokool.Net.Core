using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Sokool.Net.DataLibrary.Data;
using Sokool.Net.DataLibrary.DataAccess;

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
			//const string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SokoolNet;Integrated Security=True;";
			//services.AddDbContext<UserContext>(options => options.UseSqlServer(conn, b => b.MigrationsAssembly("Sokool.Net.DataLibrary")));

			services.AddDbContextPool<AppIdentityDbContext>(
				options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"],
					b => b.MigrationsAssembly("Sokool.Net.DataLibrary")));
			
			services.AddIdentity<AppUser, IdentityRole>(options =>
			{
				options.Password.RequiredLength = 8; 
				options.Password.RequiredUniqueChars = 3;
			}).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

			//services.Configure<IdentityOptions>(options =>
			//{
			//	options.Password.RequiredLength = 8; 
			//	options.Password.RequiredUniqueChars = 3;
			//});
			
			// The following causes the Login page to appear before any other actions can be performed
			//services.AddMvc(options =>
			//	{
			//		AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
			//			.RequireAuthenticatedUser()
			//			.Build();
			//		options.Filters.Add(new AuthorizeFilter(policy));
			//	}
			//);

			// Redirects all access denied to the Adminstration controller
			//services.ConfigureApplicationCookie(options =>
			//{
			//	options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
			//});


			services.AddAuthorization(options =>
			{
				options.AddPolicy("CreateUserPolicy", policy => policy
					.RequireClaim("CreateUser"));
				options.AddPolicy("EditUserPolicy", policy => policy
					.RequireClaim("EditUser"));
				options.AddPolicy("DeleteUserPolicy", policy => policy
					.RequireClaim("DeleteUser"));
				options.AddPolicy("ManageUserRolesPolicy", policy => policy
					.RequireClaim("ManageUserRoles"));
				options.AddPolicy("ManageUserClaimsPolicy", policy => policy
					.RequireClaim("ManageUserClaims"));
				options.AddPolicy("EditUsersInRolePolicy", policy => policy
					.RequireClaim("EditUsersInRole"));


				options.AddPolicy("CreateRolePolicy", policy => policy
					.RequireClaim("CreateRole"));
				options.AddPolicy("EditRolePolicy", policy => policy
					.RequireClaim("EditRole"));
				options.AddPolicy("DeleteRolePolicy", policy => policy
					.RequireClaim("DeleteRole"));
			});

			services.AddControllersWithViews();
#if DEBUG
			// Make sure to add the nuget package: Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation:
			services.AddRazorPages().AddRazorRuntimeCompilation();
			// Append the following to the preceding methods to temporarily disable client-side validation
			// .AddViewOptions(options =>{options.HtmlHelperOptions.ClientValidationEnabled = false;});
#endif
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CA1822 // Mark members as static
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore CA1822 // Mark members as static
		{
			if (env.IsDevelopment())
			{
				var developerExceptionPageOptions = new DeveloperExceptionPageOptions { SourceCodeLineCount = 10 };
				app.UseDeveloperExceptionPage(developerExceptionPageOptions);
			}
			else  // See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1
			{
				app.UseExceptionHandler("/Error");
				//app.UseHsts();
				app.UseStatusCodePagesWithReExecute("/Error/{0}");
			}

			//app.UseHttpsRedirection();
			
			//// The next four lines are for testing middleware pipeline. (They must come before UseStaticFiles!)
			//var fileServerOptions = new FileServerOptions();
			//fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
			//fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("Samples/centering.htm");
			//app.UseFileServer(fileServerOptions);

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

			//// Uncomment this to test.
			//app.Use(async (context, next) =>
			//{
			//	await context.Response.WriteAsync("Hello World");
			//	await next();
			//});
			// Uncomment this to test exception handling.
			//app.Run (context => throw new Exception("Some error processing the request"));

			app.UseRouting();

			// Authentication middleware
			app.UseAuthentication();

			app.UseAuthorization();

			//app.UseMvcWithDefaultRoute();

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
