namespace UrlShortener.Backend.Redirecting.Model;

using Azure;
using Azure.Data.Tables;

public class Redirection : ITableEntity
{
    public string RowKey { get; set; } = default!;

    public string PartitionKey { get; set; } = default!;

    public Type Type { get; set; } = Type.FLEXIBLE;

    public string URL { get; set; } = "";

    public ETag ETag { get; set; } = default!;

    public DateTimeOffset? Timestamp { get; set; } = default!;

    public bool IsPernament
    {
        get { return Type == Type.PERNAMENT; }
    }

    public bool IsPreservingMethod
    {
        get { return Type == Type.FLEXIBLE; }
    }
}
