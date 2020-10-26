using System.Threading.Tasks;

namespace RustyDiscordBot
{
    class Program
    {
        static async Task Main(string[] asrgs)
        {
            await new RustyDiscordBotClient().InitializeAsync();
        }

        
    }
}
