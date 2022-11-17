using Microsoft.AspNetCore.Server.IIS.Core;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace ZenyaFacadeService.HttpClient;

public class ZenyaLookupHttpClient : IZenyaLookupHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ZenyaLookupHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> FindExternalSource(string externalSourceId)
    {
        var client = _httpClientFactory.CreateClient("ZenyaClient");
        var path = $"https://msteams.zenya.work/api/external_sources/{externalSourceId}/data?include_rows=true&include_filter_list_values=false";

        var response = await client.GetAsync(path);

        if (response.StatusCode != HttpStatusCode.OK) throw new AuthenticationException();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> FindUser(string search)
    {
        var client = _httpClientFactory.CreateClient("ZenyaClient");
        var path1 = $"https://msteams.zenya.work/api/users?name={search}&limit=20";
        var path2 = $"https://msteams.zenya.work/api/teams?limit=20&offset=0&name_contains={search}";

        var responseUser = await client.GetAsync(path1);
        var responseTeam = await client.GetAsync(path2);

        if (responseUser.StatusCode != HttpStatusCode.OK) throw new AuthenticationException();
        if (responseTeam.StatusCode != HttpStatusCode.OK) throw new AuthenticationException();

        var result = "{'users':" + await responseUser.Content.ReadAsStringAsync() + ", 'teams':" + await responseTeam.Content.ReadAsStringAsync() + "}";

        return result;
    }
}
