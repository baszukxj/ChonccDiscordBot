using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Victoria;
using Victoria.EventArgs;

namespace RustyDiscordBot.EventHandlers
{
    public sealed class MusicEventHandler
    {
        public async Task OnTrackEnded(TrackEndedEventArgs args)
        {
            if (!args.Reason.ShouldPlayNext())
            {
                return;
            }

            var player = args.Player;
            if (!player.Queue.TryDequeue(out var queueable))
            {
                return;
            }

            if (!(queueable is LavaTrack track))
            {
                return;
            }

            await args.Player.PlayAsync(track);
            await args.Player.TextChannel.SendMessageAsync($"Now playing: {track.Title}");
        }
    }
}
