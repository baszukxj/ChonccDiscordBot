using System;
using System.Threading.Tasks;

namespace RustyDiscordBot
{
    class Program
    {
        static async Task Main(string[] args)
            => await new RustyDiscordBotClient().InitializeAsync();
    }
}
