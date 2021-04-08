using ChonccDiscordBot.DTOs.TwitchStream;
using ChonccDiscordBot.DTOs.TwitchUserList;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChonccDiscordBot.Services
{
    public class TwitchService
    {
        private HttpClient client = new HttpClient();

        public TwitchService()
        {
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v5+json");
            client.DefaultRequestHeaders.Add("Client-ID", GetClientId().Result);
        }

        public async Task<TwitchUsersDTO> GetTwitchUsers(string path)
        {
            using (HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    TwitchUsersDTO twitchUsers = JsonConvert.DeserializeObject<TwitchUsersDTO>(jsonResponse);

                    return twitchUsers;
                }
            }

            return null;

        }

        public async Task<bool> CheckIfStreamIsLive(string path)
        {
            using (HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    TwitchStreamDto twitchUser = JsonConvert.DeserializeObject<TwitchStreamDto>(jsonResponse);

                    if (twitchUser.Stream != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private async Task<string> GetClientId()
        {
            Secrets secrets = await ConfigureSecrets();

            return secrets.TwitchClientId;
        }

        private async Task<string> GetClientSecret()
        {
            Secrets secrets = await ConfigureSecrets();

            return secrets.TwitchClientSecret;
        }

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

}
