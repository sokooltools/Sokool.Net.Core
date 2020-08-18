using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Sokool.Net.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});

		//.ConfigureLogging(loggingBuilder =>
		//	loggingBuilder.AddFilter<ConsoleLoggerProvider>(level => level == LogLevel.Information));

		//.ConfigureLogging((loggingBuilder) =>
		//{
		//	// Clear default logging providers
		//	loggingBuilder.ClearProviders();
		//})
	}
}
