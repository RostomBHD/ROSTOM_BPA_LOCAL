using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using ROSTOM_BPA_TOOLS.Connectors;



IConfiguration configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("C:\\Users\\aph-sap\\source\\repos\\ROSTOM_BPA_LOCAL\\ROSTOM_BPA_LOCAL\\ROSTOM_BPA\\appconfig.json") // Adjust the path as necessary
            .Build();

        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

// Set up dependency injection for ILogger
var serviceProvider = new ServiceCollection()
    .AddLogging(builder => builder.AddConsole())
    .BuildServiceProvider();

var logger = serviceProvider.GetService<ILogger<SAPServiceLayerConnector>>();

// Create an instance of SAPServiceLayerConnector with ILogger
var connector = new SAPServiceLayerConnector(configuration, logger);

// Call the LoginAsync function to authenticate
string loginResponse = await connector.LoginAsync();


// Assuming "BusinessPartners" is the correct endpoint
string ordersJson = await connector.GetEntityAsync("PurchaseOrders(123)");

Console.WriteLine(ordersJson);

