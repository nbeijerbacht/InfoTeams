using FormService.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using ZenyaFacadeService.HttpClient;

namespace ZenyaFacadeTest
{
    public class ZenyaFacadeServiceControllerTest
    {

        [Fact]
        public async Task GET_forms_by_search()
        {
            await using var application = new ZenyaFacadeFactory();
            using var client = application.CreateClient();
            var response = await client.GetAsync("reporterForm?search=fontys");
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Regex.Replace(MockData.searchResult, @"\s+", ""), Regex.Replace(json, @"\s+", ""));
        }

        [Fact]
        public async Task GET_forms_by_search_nothing_found()
        {
            await using var application = new ZenyaFacadeFactory();
            using var client = application.CreateClient();
            var response = await client.GetAsync("reporterForm?search=nothing");
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("[]", json);
        }

        [Fact]
        public async Task GET_forms_by_id()
        {
            await using var application = new ZenyaFacadeFactory();
            using var client = application.CreateClient();
            var response = await client.GetAsync("reporterForm/2216");
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(MockData.mockFormById, json);
        }

        [Fact]
        public async Task GET_forms_by_id_nothing_found()
        {
            await using var application = new ZenyaFacadeFactory();
            using var client = application.CreateClient();
            var response = await client.GetAsync("reporterForm/9999");
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Not found", json);
        }

        [Fact]
        public async Task POST_forms_by_id()
        {
            await using var application = new ZenyaFacadeFactory();
            using var client = application.CreateClient();
            var response = await client.PostAsJsonAsync($"reporterForm/", "");
            var json = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}