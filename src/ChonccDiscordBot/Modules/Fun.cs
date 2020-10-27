using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace ChonccDiscordBot.Modules
{
    public class Fun : ModuleBase<SocketCommandContext>
    {
        private int _randomNumber;
        private string _roast;
        private Random random = new Random();
        private string[] roastList =
            {
                "You're weirdchamp.",
                "You're unpog.",
                "Shut up Tyler.",
                "You obviously play smite.",
                "Oompla Loompa lookin ass BITCH.",
                "Weeabo bitch.",
                "How do you convince yourself of the bullshit that comes out of your mouth?",
                "How's the simping going?",
                "You look like Xavier.",
                "You look like Dan.",
                "You look like Ian.",
                "You are british",
                "You sound british",
                "Fookin Schtewpid Wanker.",
                "You look like Rachel.",
                "You look like Nathan",
                "You look like Tyler",
                "You look like Joseph",
                "You were carried to gold.",
                "Tyler ganks more than you",
                "I'd rather have Jared jungle than you",
                "Fucking narc.",
                "You look italian",
                "Jerry is better than you at league of legends",
                "Imagine being filipino.",
                "You're a ching chong lose wong SB soraka.",
                "Pimple chin ass boi.",
                "You're bad lol.",
                "You have dumb poopy stinky brain.",
                "*Plays CryingBabyAutotune.mp4*",
                "You're more toxic then tyler and xavier in the same call.",
                "PISS BABY PISS BABY PISS BABY.",
                "Bonezone female moment.",
                "You look so much thinner today!",
                "Billy on twisted fate is better than you.",
                "Nathan on rammus is better than you",
                "Jerry on Leona is better than you.",
                "You're more toxic than JP in a tank matchup.",
                "I'd rather have Dan as the jungle then you.",
                "You must be a yuumi main.",
                "Don't you have something better to do?"
            };


        public Fun()
        {
        }

        [Command("Simp")]
        public async Task RateSimpAsync(string user = null)
        {
            _randomNumber = random.Next(0, 100);
            var embed = new EmbedBuilder { Title = "simp r8 machine" };

            embed.WithColor(Color.Blue).Build();

            if (user != null)
            {
                embed.Description = ($"{user} is {_randomNumber}% simp:smirk:");

                if (_randomNumber > 97 || (user.Equals("<@!295040512690225164>")))
                {
                    embed.Description = $"{user} is 100% giga turbo simp:smirk:";
                }
            }
            else
            {
                embed.Description = ($" You are {_randomNumber}% simp:smirk:");
                if (_randomNumber > 97 || (Context.User.Username.Equals("araize")))
                {
                    embed.Description = $"You are 100% giga turbo simp:smirk:";
                }
            }


            await ReplyAsync(embed: embed.Build());
        }

        [Command("Howgay")]
        public async Task RateGayAsync(string user = null)
        {
            _randomNumber = random.Next(0, 100);
            var embed = new EmbedBuilder { Title = "gay r8 machine" };

            embed.WithColor(Color.Blue).Build();

            if (user != null)
            {
                embed.Description = ($"{user} is {_randomNumber}% gay:gay_pride_flag:");

                if (_randomNumber > 97 || (user.Equals("<@!295040512690225164>")))
                {
                    embed.Description = $"{user} is 100% gay:gay_pride_flag:";
                }
            }
            else
            {
                embed.Description = ($" You are {_randomNumber}% gay:gay_pride_flag:");
                if (_randomNumber > 97 || (Context.User.Username.Equals("araize")))
                {
                    embed.Description = $"You are 100% gay:gay_pride_flag:";
                }
            }

            await ReplyAsync(embed: embed.Build());
        }

        [Command("Roast")]
        public async Task RoastAsync(string user = null)
        {
            _roast = roastList[random.Next(0, roastList.Length - 1)];
            await ReplyAsync(_roast);
        }

        [Command("Dan")]
        public async Task DanAsync()
        {
            await ReplyAsync("https://open.spotify.com/track/6U0FIYXCQ3TGrk4tFpLrEA?si=TpcsUb2vRECFlmUYARGgwg");
        }

        [Command("SkarnerSaturday")]
        public async Task SkarnerSaturdayAsync()
        {
            await ReplyAsync("https://cdn.discordapp.com/attachments/200319010275852288/769556097643708436/zLNC2uXESrTiZhZo.mp4");
        }

        [Command("JohnPaul")]
        public async Task JohnPaulAsync()
        {
            await ReplyAsync("https://www.reddit.com/r/TeemoTalk/");
        }
    }
}
