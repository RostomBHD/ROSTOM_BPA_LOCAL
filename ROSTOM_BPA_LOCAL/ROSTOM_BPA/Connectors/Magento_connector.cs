//Magento provides a set of API endpoints to interact with it's data, simply this claaa connecta to these API and either return or send data to Magento.
// https://r-martins.github.io/m1docs/guides/v2.4/rest/list.html

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ROSTOM_BPA_TOOLS.Connectors
{


    public class MagentoConnector
    {
        private readonly string _baseUrl;
        private readonly string _accessToken;

        public MagentoConnector(string baseUrl, string accessToken)
        {
            _baseUrl = baseUrl;
            _accessToken = accessToken;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                var response = await client.GetAsync($"{_baseUrl}{endpoint}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseBody);
            }
        }

        public async Task<string> PostAsync<T>(string endpoint, T content)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                var jsonContent = new StringContent(JsonConvert.SerializeObject(content), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_baseUrl}{endpoint}", jsonContent);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        // Add methods for PUT, DELETE, etc., as necessary.
    }

}