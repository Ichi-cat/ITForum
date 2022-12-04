using Newtonsoft.Json;

namespace ITForum.Application.Common.JsonModels
{
    public class AccessToken
    {
        [JsonProperty("access_token", Required = Required.Always)]
        public string Token { get; set; } = null!;

        [JsonProperty("scope", Required = Required.Always)]
        public string Scope { get; set; } = null!;

        [JsonProperty("token_type", Required = Required.Always)]
        public string TokenType { get; set; } = null!;
    }
}
