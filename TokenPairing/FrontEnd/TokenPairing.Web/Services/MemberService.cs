using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TokenPairing.Web.Models;
using TokenPairing.Web.Services.Interfaces;

namespace TokenPairing.Web.Services
{
    public class MemberService : IMemberService
    {

        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private string baseUrl = string.Empty;

        public MemberService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
            var serviceApiSettings = _configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();
            baseUrl = serviceApiSettings.MemberUri;

        }

        public async Task<List<TokenViewModel>> GetTokenByMemberId(string member)
        {

            var response = await _client.GetAsync($"{baseUrl}/api/member/{member}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<List<TokenViewModel>>();

            return responseSuccess;
        }

        public async Task<Response<TokenViewModel>> UpdateToken(TokenViewModel tokenViewModel)
        {

            var response = await _client.PutAsJsonAsync<TokenViewModel>($"{baseUrl}/api/Member?memberId={tokenViewModel.MEMBER_ID}&empush_token={tokenViewModel.EMPUSH_TOKEN}" +
                $"&customer_id={tokenViewModel.CUSTOMER_ID}", tokenViewModel);

            var res = await response.Content.ReadFromJsonAsync<Response<TokenViewModel>>();

            return res;

        }
    }
}
