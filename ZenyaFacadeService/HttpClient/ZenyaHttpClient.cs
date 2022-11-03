using System.Net;
using System.Text.Json;

namespace ZenyaFacadeService.HttpClient;

public class ZenyaHttpClient : IZenyaHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ZenyaHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetAllForms()
    {
        var client = _httpClientFactory.CreateClient("ZenyaClient");
        var path = "https://msteams.zenya.work/api/cases/reporter_forms";

        var response = await client.GetAsync(path);

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetFormById(int id)
    {
        var client = _httpClientFactory.CreateClient("ZenyaClient");
        var path = $"https://msteams.zenya.work/api/cases/reporter_forms/{id}?include_design=true";

        var response = await client.GetAsync(path);

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<HttpResponseMessage> PostForm(JsonElement body)
    {
        var client = _httpClientFactory.CreateClient("ZenyaClient");
        var path = $"https://msteams.zenya.work/api/cases";

        return await client.PostAsJsonAsync(path, body);
    }
}
