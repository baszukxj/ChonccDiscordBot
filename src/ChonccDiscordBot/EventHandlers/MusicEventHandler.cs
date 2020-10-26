using Discord;
using System.Threading.Tasks;
using Victoria;
using Victoria.EventArgs;

namespace ChonccDiscordBot.EventHandlers
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
            var embed = new EmbedBuilder { Description = track.Title, Title = "Now playing" };
            embed.WithColor(Color.Blue);
            
            await args.Player.TextChannel.SendMessageAsync(embed: embed.Build());
        }
    }
}
