using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace ChonccDiscordBot.Modules
{
    public class Fun : ModuleBase<SocketCommandContext>
    {

        [Command("Simp")]
        public async Task RateSimpAsync(string user)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100);

            var embed = new EmbedBuilder
            {
                Title = "Simp Calculator",
                Description = $"{user} is {randomNumber}% a simp!"
            };

            embed.WithColor(Color.Blue).Build();

            if (randomNumber > 97 || user.Equals("<@!295040512690225164>"))
            {
                embed.Description = $"{user} is 100% a giga turbo simp!";
            }

            await ReplyAsync(embed: embed.Build());
        }

    }
}
