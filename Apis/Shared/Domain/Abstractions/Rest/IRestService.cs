using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Shared.Domain.Abstractions.Rest
{
    public interface IRestService
    {
        Task<T> GetAsync<T>(string url, AuthenticationHeaderValue authenticationHeaderValue = null);

        Task<HttpResponseMessage> GetAsync(
            string url,
            AuthenticationHeaderValue authenticationHeaderValue = null
        );

        Task<TResponse> PostAsync<TResponse>(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class;

        Task<TResponse> PostAsync<TResponse, TRequest>(
            string url,
            TRequest request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class;

        Task<HttpResponseMessage> PostAsync(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        );

        Task<TResponse> PutAsync<TResponse>(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class;

        Task<TResponse> PutAsync<TResponse, TRequest>(
            string url,
            TRequest request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class;

        Task<HttpResponseMessage> PutAsync(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        );

        Task<TResponse> PatchAsync<TResponse>(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class;

        Task<TResponse> PatchAsync<TResponse, TRequest>(
            string url,
            TRequest request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class;

        Task<HttpResponseMessage> PatchAsync(
            string url,
            object request,
            AuthenticationHeaderValue authenticationHeaderValue = null
        );

        Task<TResponse> DeleteAsync<TResponse>(
            string url,
            AuthenticationHeaderValue authenticationHeaderValue = null
        )
            where TResponse : class;

        Task<HttpResponseMessage> DeleteAsync(
            string url,
            AuthenticationHeaderValue authenticationHeaderValue = null
        );
    }
}
