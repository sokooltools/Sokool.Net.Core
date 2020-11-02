using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace Sokool.Net.Web
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			try
			{
				logger.Debug("init main");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				//NLog: catch setup errors
				logger.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally
			{
				// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
				NLog.LogManager.Shutdown();
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.ConfigureLogging(loggingBuilder =>
				{
					loggingBuilder.ClearProviders();
					loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
				})
				.UseNLog();  // NLog: Setup NLog for Dependency injection

		//.ConfigureLogging(loggingBuilder =>
		//	loggingBuilder.AddFilter<ConsoleLoggerProvider>(level => level == LogLevel.Information));

		//.ConfigureLogging((loggingBuilder) =>
		//{
		//	// Clear default logging providers
		//	loggingBuilder.ClearProviders();
		//})
	}
}
