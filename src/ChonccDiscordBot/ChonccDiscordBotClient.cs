using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ChonccDiscordBot.EventHandlers;
using System;
using System.Threading.Tasks;
using Victoria;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using ChonccDiscordBot.Adapters;

namespace ChonccDiscordBot
{
    public class ChonccDiscordBotClient
    {
        private DiscordSocketClient _client;
        private CommandService _cmdService;
        private IServiceProvider _services;

        public ChonccDiscordBotClient(DiscordSocketClient client = null, CommandService cmdService = null)
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
            var secrets = await ConfigureSecrets();

            await _client.LoginAsync(TokenType.Bot, secrets.DiscordBotSecret);
            await _client.StartAsync();
            _client.Log += LogAsync;
            _client.Ready += OnReadyAsync;
            

            var cmdHandler = new CommandHandler(_client, _cmdService, _services);
            await cmdHandler.InitializeAsync();

            await Task.Delay(-1);
        }

        private async Task OnReadyAsync()
        {
            MusicEventHandler musicEventHandler = new MusicEventHandler();

            if (! (_services.GetService(typeof(LavaNode)) as LavaNode).IsConnected)
            {
                await (_services.GetService(typeof(LavaNode)) as LavaNode).ConnectAsync();          
                (_services.GetService(typeof(LavaNode)) as LavaNode).OnTrackEnded += musicEventHandler.OnTrackEnded;

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
            .AddSingleton(new TwitchAdapter())
            .AddLavaNode(x => {
                x.SelfDeaf = true;
                x.Port = 2332;
            })
            .AddLogging()
            .BuildServiceProvider();

        private async Task<Secrets> ConfigureSecrets()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("secrets.json"))
            {
                using var sr = new StreamReader(fs, new UTF8Encoding(false));
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
            }

            return JsonConvert.DeserializeObject<Secrets>(json);
        }
    }

    public class Secrets
    {
        public string DiscordBotSecret { get; set; }
        public string TwitchClientSecret { get; set; }
        public string TwitchClientId { get; set; }
    }
}
