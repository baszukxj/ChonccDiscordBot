using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RustyDiscordBot.Services;
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
            _services = SetupServices();

            await _client.LoginAsync(TokenType.Bot, "");
            await _client.StartAsync();
            _client.Log += LogAsync;
            _client.Ready += OnReadyAsync;
            

            var cmdHandler = new CommandHandler(_client, _cmdService, _services);
            await cmdHandler.InitializeAsync();

            await Task.Delay(-1);
        }

        private async Task OnReadyAsync()
        {
            if (! (_services.GetService(typeof(LavaNode)) as LavaNode).IsConnected)
            {
                await (_services.GetService(typeof(LavaNode)) as LavaNode).ConnectAsync();
                return;
            }
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
            .AddSingleton<LavaNode>()
            .AddSingleton<LavaConfig>()
            .AddSingleton<MusicService>()
            .AddLogging()
            .BuildServiceProvider();
    }
}
