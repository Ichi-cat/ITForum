using Newtonsoft.Json;

namespace ITForum.Application.Common.JsonModels
{
    public partial class FacebookUserInformation
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; } = null!;

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; } = null!;

        [JsonProperty("email", Required = Required.Always)]
        public string Email { get; set; } = null!;
    }
}
