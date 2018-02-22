using System.Threading.Tasks;

namespace Linguo
{
	public class Program
	{
		public static Task Main(string[] args)
			=> Startup.RunAsync(args);
	}
}
