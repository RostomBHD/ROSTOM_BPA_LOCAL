using System;
using System.Data;
using ROSTOM_BPA_TOOLS.Connectors;
using ROSTOM_BPA_TOOLS.Input;
using Microsoft.Extensions.Configuration;
using System.IO;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("C:\\Users\\aph-sap\\source\\repos\\ROSTOM_BPA_LOCAL\\ROSTOM_BPA_LOCAL\\ROSTOM_BPA\\appconfig.json") 
    .Build();

string dbConnectionString = configuration.GetConnectionString("MyDbConnection");

DatabaseQuery query = new DatabaseQuery();
DataTable result = query.ExecuteQuery(dbConnectionString, "SELECT * FROM OCRD");
result.TableName = "MyTable";

SqlRecordsetToXmlConverter converter = new SqlRecordsetToXmlConverter();
string xmlString = converter.ConvertToXml(result);

Console.WriteLine("XML Data:");
Console.WriteLine(xmlString);
