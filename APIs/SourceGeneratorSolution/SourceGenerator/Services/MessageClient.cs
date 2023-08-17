using Microsoft.Extensions.Configuration;

namespace SourceGenerator.Services;

public sealed class MessageClient
{
    readonly HttpClient _client;
    readonly string _url;

    public MessageClient(HttpClient client, IConfiguration configuration)
    {
        _client = client;

        string path = "Clients:MessageClientUrl";

        _url = configuration.GetValue<string>(path) 
            ?? throw new Exception($"Appsettingsben nem található a {nameof(MessageClient)} elérési útja. Út: {path}.");
    }

    public async Task<object> TestAsync()
    {
        var result = await _client.GetAsync(_url);

        return result;
    }
}