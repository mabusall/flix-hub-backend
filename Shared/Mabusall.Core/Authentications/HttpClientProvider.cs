namespace Mabusall.Core.Authentications;

public static class HttpClientProvider
{
    public static HttpClient AttachBasicAuthentication(this HttpClient client, string userName, string password)
    {
        var credentials = Encoding.ASCII.GetBytes($"{userName}:{password}");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
        return client;
    }

    public static HttpClient AttachBearerAuthentication(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public static HttpClient AttachAppiKeyAuthentication(this HttpClient client, string key, string value)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key, value);
        return client;
    }
}
