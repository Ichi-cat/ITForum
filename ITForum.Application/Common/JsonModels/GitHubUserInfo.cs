using Newtonsoft.Json;

namespace ITForum.Application.Common.JsonModels
{
    public class GitHubUserInfo
    {
        [JsonProperty("login", Required = Required.Always)]
        public string Login { get; set; } = null!;
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }
        [JsonProperty("avatar_url", Required = Required.Always)]
        public string AvatarUrl { get; set; } = null!;
        [JsonProperty("url", Required = Required.Always)]
        public string Url { get; set; } = null!;
        [JsonProperty("html_url", Required = Required.Always)]
        public string HtmlUrl { get; set; } = null!;
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? Name { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string? Email { get; set; }
        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string? Location { get; set; }
        [JsonProperty("created_at", Required = Required.Always)]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at", Required = Required.Always)]
        public DateTime UpdatedAt { get; set; }
    }
}
