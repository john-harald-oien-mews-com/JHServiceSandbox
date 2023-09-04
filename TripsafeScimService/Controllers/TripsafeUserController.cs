using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace TripsafeScimService.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TripsafeUserController : ControllerBase
    {
        private readonly ILogger<TripsafeUserController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public TripsafeUserController(ILogger<TripsafeUserController> logger, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _configuration = configuration;
            _client = _clientFactory.CreateClient("Tripsafe");
        }



        [HttpGet(Name = "Users")]
        public async Task<IActionResult> Get()
        {
            var token = await GetAuth0Token();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("api/Users");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<string> GetAuth0Token()
        {
            using var client = new HttpClient();

            var client_id = _configuration["Auth0:ClientId"];
            var client_secret = _configuration["Auth0:ClientSecret"];
            var audience = _configuration["Auth0:Audience"];
            var domain = _configuration["Auth0:Domain"];

            var requestPayload = new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
                {"client_id", client_id},
                {"client_secret", client_secret},
                {"audience", audience}
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{domain}/oauth/token")
            {
                Content = new FormUrlEncodedContent(requestPayload)
            };

            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var auth0TokenResponse = JsonConvert.DeserializeObject<Auth0TokenResponse>(responseContent);

            return auth0TokenResponse?.access_token;
        }

        private class Auth0TokenResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            // Other properties like "expires_in" can be added as needed
        }
    }
}
