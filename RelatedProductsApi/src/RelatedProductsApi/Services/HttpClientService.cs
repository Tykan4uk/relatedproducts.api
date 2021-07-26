using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using RelatedProductsApi.Services.Abstractions;

namespace RelatedProductsApi.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClientService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<TResponse> SendAsync<TResponse>(string url, HttpMethod method, object content = null)
            where TResponse : class
        {
            // discover endpoints from metadata
            var genTokenClient = new HttpClient();
            var disco = await genTokenClient.GetDiscoveryDocumentAsync("http://localhost:5000");

            // request token
            var tokenResponse = await genTokenClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "relatedproductsapi_client",
                ClientSecret = "relatedproductsapisecret"
            });

            // call api
            var client = _clientFactory.CreateClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var httpMessage = new HttpRequestMessage();
            httpMessage.RequestUri = new Uri(url);
            httpMessage.Method = method;

            if (content != null)
            {
                httpMessage.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            }

            var result = await client.SendAsync(httpMessage);

            if (result.IsSuccessStatusCode)
            {
                var resultContent = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TResponse>(resultContent);
                return response;
            }

            return null;
        }
    }
}
