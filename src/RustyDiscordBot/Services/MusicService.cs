
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Victoria;
using Victoria.EventArgs;

namespace RustyDiscordBot.Services
{
    public class MusicService
    {
        private readonly LavaNode _lavaNode;
        public readonly HashSet<ulong> VoteQueue;

        public MusicService(DiscordSocketClient socketClient, LavaNode lavaNode)
        {
            socketClient.Ready += OnReady;
            _lavaNode = lavaNode;
            _lavaNode.OnPlayerUpdated += OnPlayerUpdated;
            _lavaNode.OnStatsReceived += OnStatsReceived;
            _lavaNode.OnTrackEnded += OnTrackEnded;
            _lavaNode.OnTrackException += OnTrackException;
            _lavaNode.OnTrackStuck += OnTrackStuck;
            _lavaNode.OnWebSocketClosed += OnWebSocketClosed;

            VoteQueue = new HashSet<ulong>();
            
        }

        private Task OnPlayerUpdated(PlayerUpdateEventArgs arg)
        {
            Console.WriteLine($"Player update received for {arg.Player.VoiceChannel.Name}.");
            return Task.CompletedTask;
        }

        private Task OnStatsReceived(StatsEventArgs arg)
        {
            Console.WriteLine($"Lavalink Uptime {arg.Uptime}.");
            return Task.CompletedTask;
        }

        private async Task OnTrackEnded(TrackEndedEventArgs args)
        {
            if (!args.Reason.ShouldPlayNext())
                return;

            var player = args.Player;
            if (!player.Queue.TryDequeue(out var queueable))
            {
                await player.TextChannel.SendMessageAsync("No more tracks to play.");
                return;
            }

            if (!(queueable is LavaTrack track))
            {
                await player.TextChannel.SendMessageAsync("Next item in queue is not a track.");
                return;
            }

            await args.Player.PlayAsync(track);
            await args.Player.TextChannel.SendMessageAsync(
                $"{args.Reason}: {args.Track.Title}\nNow playing: {track.Title}");
        }

        private Task OnTrackException(TrackExceptionEventArgs arg)
        {
            Console.WriteLine($"Track exception received for {arg.Track.Title}.");
            return Task.CompletedTask;
        }

        private Task OnTrackStuck(TrackStuckEventArgs arg)
        {
            Console.WriteLine($"Track stuck received for {arg.Track.Title}.");
            return Task.CompletedTask;
        }

        private Task OnWebSocketClosed(WebSocketClosedEventArgs arg)
        {
            Console.WriteLine($"Discord WebSocket connection closed with following reason: {arg.Reason}");
            return Task.CompletedTask;
        }

        private async Task OnReady()
        {
            await _lavaNode.ConnectAsync();
        }
    }

}
