using System.Net.Http;

namespace CallerWebSite.Http
{
    public interface IHttpClientFactory
    {
        HttpClient Create(string baseUri);
    }
}