namespace FlixHub.Shared.Helper;

public interface IApiClient
{
    Task<T> GetAsync<T>(string baseUrl,
                        string endpoint,
                        Dictionary<string, string> headers = null,
                        Dictionary<string, string> query = null,
                        CancellationToken cancellationToken = default);
}

public class ApiClient(HttpClient httpClient) : IApiClient
{
    public async Task<T> GetAsync<T>(string baseUrl,
                                     string endpoint,
                                     Dictionary<string, string> headers = null,
                                     Dictionary<string, string> query = null,
                                     CancellationToken cancellationToken = default)
    {
        var url = $"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";

        if (query != null && query.Count > 0)
        {
            var qs = string.Join("&", query.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            url = $"{url}?{qs}";
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        if (headers is not null)
        {
            foreach (var (key, value) in headers)
            {
                request.Headers.TryAddWithoutValidation(key, value);
            }
        }

        using var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializerHandler.Deserialize<T>(json);
    }
}