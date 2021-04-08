
namespace ChonccDiscordBot.DTOs.TwitchStream
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class TwitchStreamDto
    {
        [JsonProperty("stream")]
        public Stream Stream { get; set; }
    }

    public partial class Stream
    {
        [JsonProperty("_id")]
        public long Id { get; set; }

        [JsonProperty("game")]
        public string Game { get; set; }

        [JsonProperty("viewers")]
        public long Viewers { get; set; }

        [JsonProperty("video_height")]
        public long VideoHeight { get; set; }

        [JsonProperty("average_fps")]
        public long AverageFps { get; set; }

        [JsonProperty("delay")]
        public long Delay { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("is_playlist")]
        public bool IsPlaylist { get; set; }

        [JsonProperty("preview")]
        public Preview Preview { get; set; }

        [JsonProperty("channel")]
        public Channel Channel { get; set; }
    }

    public partial class Channel
    {
        [JsonProperty("mature")]
        public bool Mature { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("broadcaster_language")]
        public string BroadcasterLanguage { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("game")]
        public string Game { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("_id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("partner")]
        public bool Partner { get; set; }

        [JsonProperty("logo")]
        public Uri Logo { get; set; }

        [JsonProperty("video_banner")]
        public Uri VideoBanner { get; set; }

        [JsonProperty("profile_banner")]
        public Uri ProfileBanner { get; set; }

        [JsonProperty("profile_banner_background_color")]
        public object ProfileBannerBackgroundColor { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("views")]
        public long Views { get; set; }

        [JsonProperty("followers")]
        public long Followers { get; set; }
    }

    public partial class Preview
    {
        [JsonProperty("small")]
        public Uri Small { get; set; }

        [JsonProperty("medium")]
        public Uri Medium { get; set; }

        [JsonProperty("large")]
        public Uri Large { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }
    }

    public partial class TwitchStreamDto
    {
        public static TwitchStreamDto FromJson(string json) => JsonConvert.DeserializeObject<TwitchStreamDto>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this TwitchStreamDto self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
