﻿using Microsoft.AspNetCore.Server.IIS.Core;
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
        var path = $"https://msteams.zenya.work/api/users?name={search}&limit=20";

        var response = await client.GetAsync(path);

        if (response.StatusCode != HttpStatusCode.OK) throw new AuthenticationException();

        return await response.Content.ReadAsStringAsync();
    }
}