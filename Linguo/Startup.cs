using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using System.Reflection;
using System.Linq;
using Linguo.Services;

namespace Linguo
{
	public class Startup
	{
		public IConfigurationRoot _configuration;

		public Startup(string[] args)
		{
			_configuration = new ConfigurationBuilder()
											.SetBasePath(AppContext.BaseDirectory)
											.AddJsonFile("_configuration.json")
											.Build();
		}

		public static async Task RunAsync(string[] args)
		{
			var startup = new Startup(args);
			await startup.RunAsync();
		}

		public async Task RunAsync()
		{
			var services = new ServiceCollection();
			ConfigureServices(services);

			var provider = services.BuildServiceProvider();
			provider.GetRequiredService<LoggingService>();
			provider.GetRequiredService<CommandHandler>();

			await provider.GetRequiredService<StartupService>()
										.StartAsync();


			// Prevent the program from exiting
			await Task.Delay(-1);
		}

		private void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
			{
        LogLevel = LogSeverity.Debug,     
				MessageCacheSize = 1000 // Cache 1,000 messages per channel
			}));

			services.AddSingleton(new CommandService(new CommandServiceConfig
			{
				LogLevel = LogSeverity.Verbose,     // Tell the logger to give Verbose amount of info
				DefaultRunMode = RunMode.Async, // Force all commands to run async by default
				CaseSensitiveCommands = false // Ignore case when executing commands
			}));

			services.AddSingleton<StartupService>();
			services.AddSingleton<LoggingService>();
			services.AddSingleton<CommandHandler>();
			services.AddSingleton<Random>();
			services.AddSingleton(_configuration);
		}

		//private void AddServicesSingleton(string serviceNamespace, IServiceCollection services)
		//{
		//	Assembly.GetExecutingAssembly()
		//		.GetExportedTypes()
		//		.Where(t => t.Namespace == serviceNamespace && t.GetInterfaces().Any() && t.IsClass)
		//		.Select(t => new
		//		{
		//			Interface = t.GetInterfaces().Single(i => i.Namespace == serviceNamespace),
		//			Implementation = t
		//		})
		//		.ToList()
		//		.ForEach(t => services.AddSingleton(t.Interface, t.Implementation));
		//}
	}
}
