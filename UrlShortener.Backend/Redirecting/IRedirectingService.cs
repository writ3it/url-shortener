using UrlShortener.Backend.Redirecting.Model;

namespace UrlShortener.Backend.Redirecting;

public interface IRedirectingService
{
    public Redirection CreateRedirection(string URL, Model.Type type);
    public Redirection? FindByRowKey(string rowKey);
}
