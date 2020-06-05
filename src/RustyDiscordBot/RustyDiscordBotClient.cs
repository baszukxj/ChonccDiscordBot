using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Victoria;

namespace RustyDiscordBot
{
    public class RustyDiscordBotClient
    {
        private DiscordSocketClient _client;
        private CommandService _cmdService;
        private IServiceProvider _services;

        public RustyDiscordBotClient(DiscordSocketClient client = null, CommandService cmdService = null)
        {
            _client = client ?? new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 50,
                LogLevel = LogSeverity.Debug
            });

            _cmdService = cmdService ?? new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false
            });
        }

        public async Task InitializeAsync()
        {
            await _client.LoginAsync(TokenType.Bot, "");
            await _client.StartAsync();
            _client.Log += LogAsync;
            _services = SetupServices();
            var cmdHandler = new CommandHandler(_client, _cmdService, _services);

            await cmdHandler.InitializeAsync();
            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.Message);
            return Task.CompletedTask;
        }

        private IServiceProvider SetupServices()
            => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_cmdService)
            .AddSingleton<LavaRestClient>()
            .AddSingleton<LavaSocketClient>()
            .BuildServiceProvider();
    }
}
