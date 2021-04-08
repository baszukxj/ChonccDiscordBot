using ChonccDiscordBot.Adapters;
using ChonccDiscordBot.DTOs.TwitchUserList;
using Discord;
using Discord.Commands;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChonccDiscordBot.Modules
{
    public sealed class Twitch : ModuleBase<SocketCommandContext>
    {
        private readonly TwitchAdapter twitchAdapter;

        public Twitch(TwitchAdapter adapter)
        {
            twitchAdapter = adapter;
        }

        [Command("IsLive")]
        public async Task IsLive([Remainder] string twitchUsername)
        {
            string twitchPath = "https://www.twitch.tv/";

            if (string.IsNullOrEmpty(twitchUsername))
            {
                return;
            }

            string[] userList = { twitchUsername };

            var twitchUser = await twitchAdapter.GetTwitchUsers(userList);

            if (twitchUser != null && twitchUser.Total > 0)
            {
                bool isLive = await twitchAdapter.CheckIfStreamIsLive(twitchUser.Users[0].Id.ToString());

                if (isLive)
                {
                    var embed = new EmbedBuilder
                    {
                        Title = twitchUser.Users[0].Name + " is live!",
                        Description = $"{twitchPath}{twitchUser.Users[0].Name}"
                    };

                    embed.WithColor(Color.Blue).Build();

                    await ReplyAsync(embed: embed.Build());
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"{twitchUsername} is not live!").ConfigureAwait(false);
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync($"{twitchUsername} does not exist!").ConfigureAwait(false);
            }
        }

        [Command("Track")]
        public async Task Track([Remainder] string twitchUsername)
        {
            if (string.IsNullOrEmpty(twitchUsername))
            {
                return;
            }

            string[] userList = { twitchUsername };

            var twitchUser = await twitchAdapter.GetTwitchUsers(userList);

            if (twitchUser != null && twitchUser.Total > 0)
            {
                bool isLive = await twitchAdapter.CheckIfStreamIsLive(twitchUser.Users[0].Id.ToString());

                Task.Run(() => IsLiveLoop(Context, twitchUser));

            }
            else
            {
                await Context.Channel.SendMessageAsync($"{twitchUsername} does not exist!").ConfigureAwait(false);
            }

        }

        private async Task IsLiveLoop(SocketCommandContext ctx, TwitchUsersDTO twitchUser)
        {
            string twitchPath = "https://www.twitch.tv/";
            bool isLive = false;

            while (!isLive)
            {
                Thread.Sleep(30000);

                isLive = await twitchAdapter.CheckIfStreamIsLive(twitchUser.Users[0].Id.ToString());

                if (isLive)
                {
                    var embed = new EmbedBuilder
                    {

                        Description = $"{twitchPath}{twitchUser.Users[0].Name}",
                        Title = $"{twitchUser.Users[0].Name} is live!"
                    };


                    embed.WithColor(Color.Blue).Build();


                    await ReplyAsync(embed: embed.Build());
                    //await Context.Channel.SendMessageAsync($"{ctx.Guild.EveryoneRole.Mention}").ConfigureAwait(false);

                }
            }
            while (isLive)
            {
                Thread.Sleep(30000);

                isLive = await twitchAdapter.CheckIfStreamIsLive(twitchUser.Users[0].Id.ToString());

                if (!isLive)
                {
                    await IsLiveLoop(ctx, twitchUser);
                }
            }
        }
    }
}
