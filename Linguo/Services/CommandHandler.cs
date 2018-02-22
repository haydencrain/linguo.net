using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Linguo.Services
{
	public class CommandHandler
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IConfigurationRoot _configuration;
		private readonly IServiceProvider _provider;

		public CommandHandler(
			DiscordSocketClient client,
			CommandService commands,
			IConfigurationRoot configuration,
			IServiceProvider provider)
		{
			_client = client;
			_commands = commands;
			_configuration = configuration;
			_provider = provider;

			_client.MessageReceived += OnMessageReceivedAsync;
		}

		private async Task OnMessageReceivedAsync(SocketMessage s)
		{
			var msg = s as SocketUserMessage; // ensure the message is from a user/bot
			if (msg == null) return;
			if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot) return; // Ignore self and other bots when checking commands

			var context = new SocketCommandContext(_client, msg);

			int argPos = 0;
			if (msg.HasStringPrefix(_configuration["prefix"], ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
			{
				var result = await _commands.ExecuteAsync(context, argPos, _provider);

				if (!result.IsSuccess)
					await context.Channel.SendMessageAsync(result.ToString());
			}
		}
	}
}
