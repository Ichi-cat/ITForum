using Newtonsoft.Json;

namespace ITForum.Application.Common.JsonModels
{
    public class FacebookTokenValidation
    {
        [JsonProperty("data", Required = Required.Always)]
        public Data Data { get; set; } = null!;
    }

    public class Data
    {
        [JsonProperty("app_id", Required = Required.Always)]
        public string AppId { get; set; } = null!;

        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; } = null!;

        [JsonProperty("application", Required = Required.Always)]
        public string Application { get; set; } = null!;

        [JsonProperty("data_access_expires_at", Required = Required.Always)]
        public long DataAccessExpiresAt { get; set; }

        [JsonProperty("expires_at", Required = Required.Always)]
        public long ExpiresAt { get; set; }

        [JsonProperty("is_valid", Required = Required.Always)]
        public bool IsValid { get; set; }

        [JsonProperty("scopes", Required = Required.Always)]
        public string[] Scopes { get; set; } = null!;

        [JsonProperty("user_id", Required = Required.Always)]
        public string UserId { get; set; } = null!;
    }
}
