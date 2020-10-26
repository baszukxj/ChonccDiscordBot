using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RustyDiscordBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victoria;
using Victoria.Enums;
using Victoria.EventArgs;

namespace RustyDiscordBot.Modules
{
    public sealed class Music : ModuleBase<SocketCommandContext>
    {
        private readonly LavaNode _lavaNode;

        public Music(LavaNode lavaNode)
        {
            _lavaNode = lavaNode;          
        }

        [Command("Join")]
        public async Task JoinAsync()
        {
            if (_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm already connected to a voice channel!");
                return;
            }

            var voiceState = Context.User as IVoiceState;
            if (voiceState?.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to a voice channel!");
                return;
            }

            try
            {
                await _lavaNode.JoinAsync(voiceState.VoiceChannel, Context.Channel as ITextChannel);
                await ReplyAsync($"Joined {voiceState.VoiceChannel.Name}!");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }


        [Command("Leave")]
        public async Task LeaveAsync()
        {
            var voiceState = Context.User as IVoiceState;
            if (voiceState?.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to a voice channel!");
                return;
            }

            try
            {
                await _lavaNode.LeaveAsync(voiceState.VoiceChannel);
                await ReplyAsync($"Disconnected from {voiceState.VoiceChannel.Name}!");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Command("Play")]
        public async Task PlayAsync([Remainder] string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                await ReplyAsync("Please provide search terms.");
                return;
            }

            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await JoinAsync();
            }


            var searchResponse = await _lavaNode.SearchYouTubeAsync(searchQuery);
            if (searchResponse.LoadStatus == LoadStatus.LoadFailed ||
                searchResponse.LoadStatus == LoadStatus.NoMatches)
            {
                await ReplyAsync($"I wasn't able to find anything for `{searchQuery}`.");
                return;
            }

            var player = _lavaNode.GetPlayer(Context.Guild);

            if (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused)
            {
                if (!string.IsNullOrWhiteSpace(searchResponse.Playlist.Name))
                {
                    foreach (var track in searchResponse.Tracks)
                    {

                        player.Queue.Enqueue(track);
                    }

                    await ReplyAsync($"Enqueued {searchResponse.Tracks.Count} tracks.");
                }
                else
                {
                    var track = searchResponse.Tracks[0];
                    player.Queue.Enqueue(track);
                    await ReplyAsync($"Enqueued: {track.Title}");
                }
            }
            else
            {
                var track = searchResponse.Tracks[0];

                if (!string.IsNullOrWhiteSpace(searchResponse.Playlist.Name))
                {
                    for (var i = 0; i < searchResponse.Tracks.Count; i++)
                    {
                        if (i == 0)
                        {
                            await player.PlayAsync(track);
                            await ReplyAsync($"Now Playing: {track.Title}");
                        }
                        else
                        {
                            player.Queue.Enqueue(searchResponse.Tracks[i]);
                        }
                    }

                    await ReplyAsync($"Enqueued {searchResponse.Tracks.Count} tracks.");
                }
                else
                {
                    await player.PlayAsync(track);
                    await ReplyAsync($"Now Playing: {track.Title}");
                }
            }

        }

        [Command("Pause")]
        public async Task PauseAsync()
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to a voice channel.");
                return;
            }

            var player = _lavaNode.GetPlayer(Context.Guild);

            try
            {
                await player.PauseAsync();
                await ReplyAsync("Queue paused.");
            }
            catch
            {
                await ReplyAsync("Error.");
            }
        }

        [Command("Resume")]
        public async Task ResumeAsync()
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to a voice channel.");
                return;
            }

            var player = _lavaNode.GetPlayer(Context.Guild);

            try
            {
                await player.ResumeAsync();
                await ReplyAsync("Queue resumed.");
            }
            catch
            {
                await ReplyAsync("Error.");
            }
        }

        [Command("Skip")]
        public async Task SkipAsync()
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to a voice channel.");
                return;
            }

            var player = _lavaNode.GetPlayer(Context.Guild);

            try
            {
                await player.SkipAsync();
                await ReplyAsync("Song skipped.");
                await ReplyAsync($"Now Playing: {player.Track.Title}");
            }
            catch
            {
                await ReplyAsync("Error.");
            }
        }

        [Command("Stop")]
        public async Task StopAsync()
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to a voice channel.");
                return;
            }

            var player = _lavaNode.GetPlayer(Context.Guild);

            try
            {
                await player.StopAsync();
                await ReplyAsync("Queue stopped.");
            }
            catch
            {
                await ReplyAsync("Error.");
            }
        }

        [Command("Queue")]
        public async Task ShowQueueAsync()
        {
            if (!_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to a voice channel.");
                return;
            }

            var player = _lavaNode.GetPlayer(Context.Guild);
            var stringBuilder = new StringBuilder();
            int queueNumber = 1;

            stringBuilder.AppendLine($"▶️{queueNumber}. {player.Track.Title} \n");

            foreach (var track in player.Queue)
            {
                queueNumber++;
                stringBuilder.AppendLine($"{queueNumber}. {track.Title}  \n");           
            }

            await ReplyAsync($"```{stringBuilder}```");
        }


        private static readonly IEnumerable<int> Range = Enumerable.Range(1900, 2000);

        [Command("Lyrics", RunMode = RunMode.Async)]
        public async Task ShowGeniusLyrics()
        {
            if (!_lavaNode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAsync("I'm not connected to a voice channel.");
                return;
            }

            if (player.PlayerState != PlayerState.Playing)
            {
                await ReplyAsync("Woaaah there, I'm not playing any tracks.");
                return;
            }

            var lyrics = await player.Track.FetchLyricsFromGeniusAsync();
            if (string.IsNullOrWhiteSpace(lyrics))
            {
                await ReplyAsync($"No lyrics found for {player.Track.Title}");
                return;
            }

            var splitLyrics = lyrics.Split('\n');
            var stringBuilder = new StringBuilder();
            foreach (var line in splitLyrics)
            {
                if (Range.Contains(stringBuilder.Length))
                {
                    await ReplyAsync($"```{stringBuilder}```");
                    stringBuilder.Clear();
                }
                else
                {
                    stringBuilder.AppendLine(line);
                }
            }

            await ReplyAsync($"```{stringBuilder}```");
        }

    }
}
