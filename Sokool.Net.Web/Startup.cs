using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
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
				//options.SignIn.RequireConfirmedEmail = true;
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
			}).AddEntityFrameworkStores<AppIdentityDbContext>()
				.AddDefaultTokenProviders();

			// The following can be used in lieu of the preceding.
			//services.Configure<IdentityOptions>(options =>
			//{
			//	options.Password.RequiredLength = 8;
			//	options.Password.RequiredUniqueChars = 3;
			//	options.SignIn.RequireConfirmedEmail = true;
			//});

			// Set (email, reset password) token life span to 5 hours
			services.Configure<DataProtectionTokenProviderOptions>(o =>
				o.TokenLifespan = TimeSpan.FromHours(5));

			//// Changes token lifespan of just the Email Confirmation Token type
			//services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
			//	o.TokenLifespan = TimeSpan.FromDays(3));

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

			// Perform Authorization
			services.AddAuthorization(options =>
			{
				options.AddPolicy("CreateUserPolicy", policy => policy
					.RequireAssertion(context => AuthorizeAccess(context, "CreateUser")));
				
				options.AddPolicy("EditUserPolicy", policy => policy
					.RequireAssertion(context => AuthorizeAccess(context, "EditUser")));
				
				options.AddPolicy("DeleteUserPolicy", policy => policy
					.RequireAssertion(context => AuthorizeAccess(context, "DeleteUser")));
				
				options.AddPolicy("ManageUserRolesPolicy", policy => policy
					.RequireAssertion(context => AuthorizeAccess(context, "ManageUserRoles")));
				
				options.AddPolicy("ManageUserClaimsPolicy", policy => policy
					.RequireAssertion(context => AuthorizeAccess(context, "ManageUserClaims")));
				
				options.AddPolicy("EditUsersInRolePolicy", policy => policy
					.RequireAssertion(context => AuthorizeAccess(context, "EditUsersInRole")));
				
				options.AddPolicy("CreateRolePolicy", policy => policy
					.RequireAssertion(context => AuthorizeAccess(context, "CreateRole")));

				options.AddPolicy("EditRolePolicy", policy => policy
					.RequireAssertion(context => AuthorizeAccess(context, "EditRole")));

				options.AddPolicy("DeleteRolePolicy", policy => policy
					.RequireAssertion(context => AuthorizeAccess(context, "DeleteRole")));
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
			//if (env.IsDevelopment())
			//{
			//	var developerExceptionPageOptions = new DeveloperExceptionPageOptions { SourceCodeLineCount = 10 };
			//	app.UseDeveloperExceptionPage(developerExceptionPageOptions);
			//}
			//else  // See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1
			//{
			app.UseExceptionHandler("/Error");
			//app.UseHsts();
			app.UseStatusCodePagesWithReExecute("/Error/{0}");
			//}

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

		private static bool AuthorizeAccess(AuthorizationHandlerContext context, string claimType)
		{
			return context.User.IsInRole("Admin") &&
				   context.User.HasClaim(claim => claim.Type == claimType && claim.Value == "true") ||
				   context.User.IsInRole("SuperUser");
		}
	}
}
