using ITForum.Application.Common.JsonModels;
using ITForum.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ITForum.Application.Services
{
    public class GitHubAuthentication : IGitHubAuthentication
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        private const string AccessTokenUrl = "https://github.com/login/oauth/access_token?client_id={0}&client_secret={1}&code={2}";
        private const string GetUserInfoUrl = "https://api.github.com/user";
        private const string GetUserEmailUrl = "https://api.github.com/user/emails";
        public GitHubAuthentication(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<AccessToken> GetAccessToken(string code)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ITForum", _configuration["ITForumVersion"]));
            var data = await client
                .PostAsync(String.Format(AccessTokenUrl,
                _configuration["Authentication:GitHub:ClientId"], _configuration["Authentication:GitHub:ClientSecret"], code), null);
            var result = await data.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AccessToken>(result);
        }

        public async Task<GitHubUserInfo> GetUserInformation(string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ITForum", _configuration["ITForumVersion"]));
            var data = await client.GetAsync(GetUserInfoUrl);
            var result = await data.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GitHubUserInfo>(result);
        }

        public async Task<IEnumerable<GitHubUserEmail>> GetUserEmails(string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            //dry
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ITForum", _configuration["ITForumVersion"]));
            var data = await client.GetAsync(GetUserEmailUrl);
            var result = await data.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GitHubUserEmail[]>(result);
        }
    }
}
