using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Victoria;
using Victoria.Entities;

namespace RustyDiscordBot.Services
{
    public class MusicService
    {
        private LavaRestClient _lavaNode;
        private LavaSocketClient _lavaSocket;
        private DiscordSocketClient _client;

        public MusicService(LavaRestClient lavaNode, LavaSocketClient lavaSocket, DiscordSocketClient client)
        {
            _lavaNode = lavaNode;
            _lavaSocket = lavaSocket;
            _client = client;
        }

        public Task InitializeAsync()
        {
            _client.Ready += ClientReadyAsync;
            _lavaSocket.Log += LogAsync;
            _lavaSocket.OnTrackFinished += TrackFinished;
            return Task.CompletedTask;           
        }

        private async Task TrackFinished(LavaPlayer player, LavaTrack track, TrackEndReason reason)
        {
            if (!reason.ShouldPlayNext()) 
                return;

            if(!player.Queue.TryDequeue(out var item) || !(item is LavaTrack nextTrack))
            {
                await player.TextChannel.SendMessageAsync("There are no more tracks in the queue.");
                return;
            }

            await player.PlayAsync(nextTrack);
        }

        private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }

        private async Task ClientReadyAsync()
        {
            await _lavaSocket.StartAsync(_client);
        }
    }
}
