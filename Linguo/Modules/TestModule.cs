using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Linguo.Modules
{
	[Name("Test")]
	[Summary("This is just a test module")]
	public class TestModule : ModuleBase<SocketCommandContext>
	{
		[Command("ping")]
		[Summary("basic ping pong implementation")]
		public Task PingPong()
		{ 
			return ReplyAsync("pong");
		}

		[Command("say")]
		[Summary("make linguo say something")]
		public Task Say([Remainder]string text)
		{
			return ReplyAsync(text);
		}

		[Command("dead?")]
		[Summary("make linguo say something")]
		public Task LinguoIsDead()
		{
			return ReplyAsync("Linguo IS dead");
		}
	}
}
