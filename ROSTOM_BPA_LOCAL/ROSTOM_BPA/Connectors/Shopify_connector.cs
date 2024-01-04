using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ROSTOM_BPA_TOOLS.Connectors
{
    public class ShopifyConnector
    {
        private readonly string _shopUrl;
        private readonly string _apiKey;
        private readonly string _password;
        private readonly HttpClient _client;

        public ShopifyConnector(string shopUrl, string apiKey, string password)
        {
            _shopUrl = shopUrl ?? throw new ArgumentNullException(nameof(shopUrl));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _password = password ?? throw new ArgumentNullException(nameof(password));
            _client = CreateHttpClient();
        } 

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{_apiKey}:{_password}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return client;
        }

        // Generic GET method
        public async Task<string> GetAsync(string endpoint)
        {
            var response = await _client.GetAsync($"{_shopUrl}{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Generic POST method
        public async Task<string> PostAsync(string endpoint, string contentJson)
        {
            var content = new StringContent(contentJson, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_shopUrl}{endpoint}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Generic PUT method
        public async Task<string> PutAsync(string endpoint, string contentJson)
        {
            var content = new StringContent(contentJson, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{_shopUrl}{endpoint}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Generic DELETE method
        public async Task<string> DeleteAsync(string endpoint)
        {
            var response = await _client.DeleteAsync($"{_shopUrl}{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }

}
