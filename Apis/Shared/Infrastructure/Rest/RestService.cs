using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Domain.Abstractions.Rest;

namespace Shared.Infrastructure.Rest
{
    public class RestService(HttpClient httpClient, ILogger<RestService> logger)
        : ResilientService(logger),
            IRestService
    {
        private readonly HttpClient _httpClient = httpClient;

        private static HttpContent GetHttpContent<TRequest>(TRequest request) =>
            new StringContent(
                JsonConvert.SerializeObject(request),
                Encoding.UTF8,
                "application/json"
            );

        public async Task<T> GetAsync<T>(
            string url,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
        {
            if (authenticationHeaderValue != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }

            var response = await BuildExecutionPolicy(url)
                .ExecuteAsync(async () => await _httpClient.GetAsync(url));
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }

        public async Task<HttpResponseMessage> GetAsync(
            string url,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
        {
            if (authenticationHeaderValue != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            return await BuildExecutionPolicy(url)
                .ExecuteAsync(async () => await _httpClient.GetAsync(url));
        }

        public async Task<TResponse> PostAsync<TResponse>(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class =>
            await PostAsync<TResponse, object>(url, request, authenticationHeaderValue);

        public async Task<TResponse> PostAsync<TResponse, TRequest>(
            string url,
            TRequest request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class
        {
            var content = GetHttpContent(request);
            if (authenticationHeaderValue != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            var response = await BuildExecutionPolicy(url)
                .ExecuteAsync(async () => await _httpClient.PostAsync(url, content));
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResponse>(json);
            return result;
        }

        public async Task<HttpResponseMessage> PostAsync(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
        {
            var content = GetHttpContent(request);
            if (authenticationHeaderValue != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            return await BuildExecutionPolicy(url)
                .ExecuteAsync(async () => await _httpClient.PostAsync(url, content));
        }

        public async Task<TResponse> PutAsync<TResponse>(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class =>
            await PutAsync<TResponse, object>(url, request, authenticationHeaderValue);

        public async Task<TResponse> PutAsync<TResponse, TRequest>(
            string url,
            TRequest request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class
        {
            var content = GetHttpContent(request);
            if (authenticationHeaderValue != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            var response = await BuildExecutionPolicy(url)
                .ExecuteAsync(async () => await _httpClient.PutAsync(url, content));
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResponse>(json);
            return result;
        }

        public async Task<HttpResponseMessage> PutAsync(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
        {
            var content = GetHttpContent(request);
            if (authenticationHeaderValue != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            return await _httpClient.PutAsync(url, content);
        }

        public async Task<TResponse> PatchAsync<TResponse>(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class =>
            await PatchAsync<TResponse, object>(url, request, authenticationHeaderValue);

        public async Task<TResponse> PatchAsync<TResponse, TRequest>(
            string url,
            TRequest request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class
        {
            var content = GetHttpContent(request);
            if (authenticationHeaderValue != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            var response = await BuildExecutionPolicy(url)
                .ExecuteAsync(async () => await _httpClient.PatchAsync(url, content));
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResponse>(json);
            return result;
        }

        public async Task<HttpResponseMessage> PatchAsync(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
        {
            var content = GetHttpContent(request);
            if (authenticationHeaderValue != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            return await BuildExecutionPolicy(url)
                .ExecuteAsync(async () => await _httpClient.PatchAsync(url, content));
        }

        public async Task<TResponse> DeleteAsync<TResponse>(
            string url,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class
        {
            if (authenticationHeaderValue != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            var response = await BuildExecutionPolicy(url)
                .ExecuteAsync(async () => await _httpClient.DeleteAsync(url));
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResponse>(json);
            return result;
        }

        public async Task<HttpResponseMessage> DeleteAsync(
            string url,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
        {
            if (authenticationHeaderValue != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            return await BuildExecutionPolicy(url)
                .ExecuteAsync(async () => await _httpClient.DeleteAsync(url));
        }
    }
}
