using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using UrlShortener.Backend.Redirecting.Model;
using UrlShortener.Backend.Util;

namespace UrlShortener.Backend.Redirecting.Service;

class RedirectingService : IRedirectingService
{
    public const string defaultPartition = "all";
    private const string tableName = "redirection";

    private static readonly Dictionary<string, TableClient> clients =
        new Dictionary<string, TableClient>();
    private readonly string? connectionString;

    public RedirectingService()
    {
        connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
    }

    public Redirection CreateRedirection(string URL, Model.Type type)
    {
        var redirection = new Redirection
        {
            PartitionKey = defaultPartition,
            RowKey = GenerateUniqueToken(URL),
            Type = type,
            URL = URL
        };
        var response = GetClient(tableName).AddEntity<Redirection>(redirection);
        Console.WriteLine(response.Status);
        return redirection;
    }

    public Redirection? FindByRowKey(string rowKey)
    {
        try
        {
            var response = GetClient(tableName).GetEntity<Redirection>(defaultPartition, rowKey);
            Console.WriteLine(rowKey);
            if (!response.HasValue)
            {
                return null;
            }
            return response.Value;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public TableClient GetClient(string tableName)
    {
        TableClient? client;
        if (!clients.TryGetValue(tableName, out client))
        {
            var service = new TableServiceClient(connectionString);
            client = service.GetTableClient(tableName);
            clients.Add(tableName, client);
        }
        return client;
    }

    private string GenerateUniqueToken(string uRL)
    {
        // TODO: improve, avoid duplications;
        var ticks = DateTime.UtcNow.Ticks;
        var token = NumberConverter.ConvertToSpace(ticks);
        char[] charArray = token.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
