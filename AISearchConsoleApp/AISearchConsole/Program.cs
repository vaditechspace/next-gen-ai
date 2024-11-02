using AISearchService;
using Azure.Search.Documents.Models;
using Azure;
using Microsoft.Extensions.Configuration;
using System.IO;

OpenAISearchRequest openAISearchRequest = new OpenAISearchRequest();
PopulateAIRequest(ref openAISearchRequest);

VectorAISearch vais = new VectorAISearch(openAISearchRequest);

while (true)
{
    Console.Write("Enter your search query: "); 
    string input = Console.ReadLine();
    if (string.IsNullOrEmpty(input))
    {
        Console.WriteLine("Please enter a valid query.");
        continue;
    }
    var results = await vais.VectorAISearchAsync(input, "MIS");
    if (string.IsNullOrEmpty(results))
    {
        Console.WriteLine("No results found.");
        continue;
    }
    Console.WriteLine(results);
    Console.WriteLine("\r\n-------------------------");
}

static void PopulateAIRequest(ref OpenAISearchRequest openAISearchRequest)
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    IConfiguration configuration = builder.Build();

    //Azure OpenAI Embed
    openAISearchRequest.AzOpenAIEmbedUri = configuration["AzOAIEmbedUri"]; ;
    openAISearchRequest.AzOpenAIEmbedApiKey = configuration["AzOAIEmbedKey"];
    openAISearchRequest.AzOpenAIEmbedModel = configuration["AzOAIEmbedModel"];


    //Azure AI Search
    openAISearchRequest.AzAISearchURI = configuration["AISEndPoint"];
    openAISearchRequest.AzAISearchKey = configuration["AISKey"];
    openAISearchRequest.AzAISearchIndex = configuration["AzAISearchIndex"];
}
