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
    private readonly ILogger<SAPServiceLayerConnector> logger;

    public SAPServiceLayerConnector(IConfiguration configuration, ILogger<SAPServiceLayerConnector> logger)
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        httpClient = new HttpClient(handler);

        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var serviceLayerConfig = this.configuration.GetSection("SAPServiceLayerConfig");
        var serviceLayerUrl = serviceLayerConfig["ServiceLayerUrl"];

        httpClient.BaseAddress = new Uri(serviceLayerUrl);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        logger.LogInformation("SAPServiceLayerConnector initialized with URL: {Url}", serviceLayerUrl);
    }

    public async Task<string> LoginAsync()
    {
        var serviceLayerConfig = configuration.GetSection("SAPServiceLayerConfig");
        var companyDB = serviceLayerConfig["CompanyDB"];
        var username = serviceLayerConfig["Username"];
        var password = serviceLayerConfig["Password"];

        var loginInfo = new { CompanyDB = companyDB, UserName = username, Password = password };

        var jsonContent = JsonConvert.SerializeObject(loginInfo);


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
            logger.LogInformation($"Attempting to retrieve data from {endpoint}");

            // Correctly append the endpoint to the base URL
            string fullUrl = httpClient.BaseAddress.ToString().TrimEnd('/') + "/" + endpoint.TrimStart('/');
            HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

            response.EnsureSuccessStatusCode();
            logger.LogInformation($"Data retrieved successfully from {endpoint}");

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, $"Error retrieving data from {endpoint}");
            throw;
        }
    }


    public async Task<string> CreateEntityAsync<T>(string endpoint, T entity)
    {
        try
        {
            logger.LogInformation($"Attempting to create entity at {endpoint}");
            string fullUrl = httpClient.BaseAddress.ToString().TrimEnd('/') + "/" + endpoint.TrimStart('/');
            var jsonContent = JsonConvert.SerializeObject(entity);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(fullUrl, content);
            response.EnsureSuccessStatusCode();
            logger.LogInformation($"Successfully created entity at {endpoint}");
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, $"Error creating entity at {endpoint}");
            throw;
        }
    }

    public async Task UpdateEntityAsync<T>(string endpoint, T entity)
    {
        try
        {
            logger.LogInformation($"Attempting to update entity at {endpoint}");
            string fullUrl = httpClient.BaseAddress.ToString().TrimEnd('/') + "/" + endpoint.TrimStart('/');
            var jsonContent = JsonConvert.SerializeObject(entity);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PatchAsync(fullUrl, content);
            response.EnsureSuccessStatusCode();
            logger.LogInformation($"Entity updated successfully at {endpoint}");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, $"Error updating entity at {endpoint}");
            throw;
        }
    }

    public async Task DeleteEntityAsync(string endpoint)
    {
        try
        {
            logger.LogInformation($"Attempting to delete entity at {endpoint}");
            string fullUrl = httpClient.BaseAddress.ToString().TrimEnd('/') + "/" + endpoint.TrimStart('/');
            HttpResponseMessage response = await httpClient.DeleteAsync(fullUrl);
            response.EnsureSuccessStatusCode();
            logger.LogInformation($"Entity deleted successfully at {endpoint}");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, $"Error deleting entity at {endpoint}");
            throw;
        }
    }

    public void Dispose()
    {
        httpClient?.Dispose();
        logger.LogInformation("HttpClient disposed");
    }
}
