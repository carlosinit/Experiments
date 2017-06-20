using System;
using System.Net.Http;

namespace CallerWebSite.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly Func<HttpMessageHandler> _httpMessageHandlerFactoryMethod;

        public HttpClientFactory(Func<HttpMessageHandler> httpMessageHandlerFactoryMethod)
        {
            _httpMessageHandlerFactoryMethod = httpMessageHandlerFactoryMethod;
        }

        public HttpClient Create(string baseUri)
        {
            if (_httpMessageHandlerFactoryMethod != null)
            {
                return new HttpClient(_httpMessageHandlerFactoryMethod()) { BaseAddress = new Uri(baseUri) };
            }

            return new HttpClient { BaseAddress = new Uri(baseUri) };
        }
    }
}