using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace RustyDiscordBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdService;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordSocketClient client, CommandService cmdService, IServiceProvider services)
        {
            _client = client;
            _cmdService = cmdService;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _cmdService.AddModulesAsync(Assembly.GetExecutingAssembly(), _services);
            _cmdService.Log += LogAsync;
            _client.MessageReceived += HandleMessageAsync;
        }

        private async Task HandleMessageAsync(SocketMessage msg)
        {
            var argPos = 0;
            var userMessage = msg as SocketUserMessage;

            if (userMessage is null) return;

            if ((!userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos)               
                && !userMessage.HasCharPrefix('!', ref argPos))
                 || msg.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, userMessage);
            var result = await _cmdService.ExecuteAsync(context, argPos, _services);
        }

        private Task LogAsync(Discord.LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }
    }
}
