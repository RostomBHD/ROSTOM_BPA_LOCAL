using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using ROSTOM_BPA_TOOLS.Connectors;  // Ensure this using directive is correct


    
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("C:\\Users\\aph-sap\\source\\repos\\ROSTOM_BPA_LOCAL\\ROSTOM_BPA_LOCAL\\ROSTOM_BPA\\appconfig.json") // Adjust the path as necessary
            .Build();

        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

        var connector = new SAPServiceLayerConnector(configuration);

        // Call the LoginAsync function to authenticate
        string loginResponse = await connector.LoginAsync();
        Console.WriteLine("Login successful. Response: " + loginResponse);

        // Assuming "BusinessPartners" is the correct endpoint
        string ordersJson = await connector.GetEntityAsync("BusinessPartners");
if (!string.IsNullOrEmpty(ordersJson))
{
    Console.WriteLine("Orders retrieved successfully:");
    Console.WriteLine(ordersJson);
}
            // Additional processing of the data
