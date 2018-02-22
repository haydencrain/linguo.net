using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Linguo.Modules;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linguo.Services
{
	public class StartupService
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IConfigurationRoot _configuration;

		public StartupService(
			DiscordSocketClient client,
			CommandService commands,
			IConfigurationRoot configuration)
		{
			_client = client;
			_commands = commands;
			_configuration = configuration;
		}

		public async Task StartAsync()
		{
			string discordToken = _configuration["token"];

			if (string.IsNullOrWhiteSpace(discordToken))
				throw new Exception("bot token not found in '_configuration.json'");

			await _client.LoginAsync(TokenType.Bot, discordToken);
			await _client.StartAsync();

			//await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
			await _commands.AddModuleAsync<TestModule>();
		}
	}
}