using Newtonsoft.Json;

namespace ITForum.Application.Common.JsonModels
{
    public partial class GitHubUserEmail
    {
        [JsonProperty("email", Required = Required.Always)]
        public string Email { get; set; } = null!;

        [JsonProperty("primary", Required = Required.Always)]
        public bool Primary { get; set; }

        [JsonProperty("verified", Required = Required.Always)]
        public bool Verified { get; set; }

        [JsonProperty("visibility", NullValueHandling = NullValueHandling.Ignore)]
        public string? Visibility { get; set; }
    }
}
