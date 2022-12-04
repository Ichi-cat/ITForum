using ITForum.Application.Common.JsonModels;
using ITForum.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;

namespace ITForum.Application.Services
{
    public class FacebookAuthentication : IFacebookAuthentication
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string validateTokenUrl = "https://graph.facebook.com/v15.0/debug_token?access_token={0}|{1}&input_token={2}";
        private readonly string userInformationUrl = "https://graph.facebook.com/v15.0/me?fields=id,name,email&access_token={0}";

        private readonly IConfiguration _configuration;
        public FacebookAuthentication(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<FacebookUserInformation> GetUserInformation(string accessToken)
        {
            var data = await _httpClientFactory.CreateClient().GetAsync(String.Format(userInformationUrl, accessToken));
            var result = await data.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookUserInformation>(result);
        }

        public async Task<FacebookTokenValidation> ValidateToken(string accessToken)
        {
            var data = await _httpClientFactory.CreateClient().GetAsync(String.Format(validateTokenUrl, _configuration["Authentication:Facebook:AppId"],
                _configuration["Authentication:Facebook:AppSecret"], accessToken));
            var result = await data.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookTokenValidation>(result);
        }
    }
}
