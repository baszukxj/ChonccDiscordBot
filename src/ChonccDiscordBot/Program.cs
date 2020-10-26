using System.Threading.Tasks;

namespace ChonccDiscordBot
{
    class Program
    {
        static async Task Main(string[] asrgs)
        {
            await new ChonccDiscordBotClient().InitializeAsync();
        }

        
    }
}
