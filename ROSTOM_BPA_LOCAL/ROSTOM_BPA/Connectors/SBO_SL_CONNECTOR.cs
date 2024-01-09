using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class SAPServiceLayerConnector : IDisposable
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public SAPServiceLayerConnector(IConfiguration configuration)
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator; // SSL Bypass
        httpClient = new HttpClient(handler);

        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        var serviceLayerConfig = this.configuration.GetSection("SAPServiceLayerConfig");
        var serviceLayerUrl = serviceLayerConfig["ServiceLayerUrl"];

        httpClient.BaseAddress = new Uri(serviceLayerUrl);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> LoginAsync()
    {
        var serviceLayerConfig = configuration.GetSection("SAPServiceLayerConfig");
        var companyDB = serviceLayerConfig["CompanyDB"];
        var username = serviceLayerConfig["Username"];
        var password = serviceLayerConfig["Password"];

        var loginInfo = new { CompanyDB = companyDB, UserName = username, Password = password };

        var jsonContent = JsonConvert.SerializeObject(loginInfo);
        Console.WriteLine($"Request URL: {httpClient.BaseAddress}Login");
        Console.WriteLine($"Request Body: {jsonContent}");

        // ... existing code to send the request ...

        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            //logger.LogInformation("Attempting to log in to SAP Service Layer");

            HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress.ToString().TrimEnd('/') + "/Login", content);
            response.EnsureSuccessStatusCode();
            //logger.LogInformation("Successfully logged in to SAP Service Layer");
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            //logger.LogError(ex, "Error logging in to SAP Service Layer");
            throw;
        }
    }

    public async Task<string> GetEntityAsync(string endpoint)
    {
        try
        {
            string fullUrl = httpClient.BaseAddress.ToString().TrimEnd('/') + "/" + endpoint.TrimStart('/');
            HttpResponseMessage response = await httpClient.GetAsync(fullUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            // Handle the error
            Console.WriteLine($"Error retrieving data from {endpoint}: {e.Message}");
            return null;
        }
    }

    public async Task<string> CreateEntityAsync<T>(string endpoint, T entity)
    {
        try
        {
            string fullUrl = httpClient.BaseAddress.ToString().TrimEnd('/') + "/" + endpoint.TrimStart('/');
            var jsonContent = JsonConvert.SerializeObject(entity);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(fullUrl, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            // Handle the error
            Console.WriteLine($"Error creating entity at {endpoint}: {e.Message}");
            return null;
        }
    }

    public async Task UpdateEntityAsync<T>(string endpoint, T entity)
    {
        try
        {
            string fullUrl = httpClient.BaseAddress.ToString().TrimEnd('/') + "/" + endpoint.TrimStart('/');
            var jsonContent = JsonConvert.SerializeObject(entity);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PatchAsync(fullUrl, content);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            // Handle the error
            Console.WriteLine($"Error updating entity at {endpoint}: {e.Message}");
        }
    }

    public async Task DeleteEntityAsync(string endpoint)
    {
        try
        {
            string fullUrl = httpClient.BaseAddress.ToString().TrimEnd('/') + "/" + endpoint.TrimStart('/');
            HttpResponseMessage response = await httpClient.DeleteAsync(fullUrl);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            // Handle the error
            Console.WriteLine($"Error deleting entity at {endpoint}: {e.Message}");
        }
    }


    public void Dispose()
    {
        httpClient?.Dispose();
    }
}
