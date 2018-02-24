using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Linguo.Services
{
	public class LoggingService
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;

		private string _logDirectory { get; }
		private string _logFile => Path.Combine(_logDirectory, $"{DateTime.Now.ToString("yyyy-MM-dd")}.txt");

		public LoggingService(
			DiscordSocketClient client,
			CommandService commands)
		{
			_logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

			_client = client;
			_commands = commands;

			_client.Log += OnLogAsync;
			_commands.Log += OnLogAsync;
		}

		private Task OnLogAsync(LogMessage msg)
		{
			if (!Directory.Exists(_logDirectory))
				Directory.CreateDirectory(_logDirectory);
			if (!File.Exists(_logFile))
				File.Create(_logFile).Dispose();

			string logText = $"{DateTime.Now,-19} [{msg.Severity,8}] {msg.Source}: {msg.Message} {msg.Exception}";

			File.AppendAllText(_logFile, logText + "\n");

			Console.ResetColor();
			ChangeConsoleColor(msg.Severity);
			return Console.Out.WriteLineAsync(logText);
		}

		private void ChangeConsoleColor(LogSeverity severity)
		{
			switch (severity)
			{
				case LogSeverity.Critical:
				case LogSeverity.Error:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case LogSeverity.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case LogSeverity.Info:
				case LogSeverity.Verbose:
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case LogSeverity.Debug:
					Console.ForegroundColor = ConsoleColor.DarkGray;
					break;
			}
		}
	}
}
