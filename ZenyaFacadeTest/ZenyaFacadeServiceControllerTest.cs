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
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Regex.Replace(json, @"\s+", "").Should().Be(Regex.Replace(MockData.searchResult, @"\s+", ""));
        }

        [Fact]
        public async Task GET_forms_by_search_nothing_found()
        {
            await using var application = new ZenyaFacadeFactory();
            using var client = application.CreateClient();
            var response = await client.GetAsync("reporterForm?search=nothing");
            var json = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            json.Should().Be("[]");
        }

        [Fact]
        public async Task GET_forms_by_id()
        {
            await using var application = new ZenyaFacadeFactory();
            using var client = application.CreateClient();
            var response = await client.GetAsync("reporterForm/2216");
            var json = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            json.Should().Be(MockData.mockFormById);
        }

        [Fact]
        public async Task GET_forms_by_id_nothing_found()
        {
            await using var application = new ZenyaFacadeFactory();
            using var client = application.CreateClient();
            var response = await client.GetAsync("reporterForm/9999");
            var json = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task POST_forms_by_id()
        {
            await using var application = new ZenyaFacadeFactory();
            using var client = application.CreateClient();
            var response = await client.PostAsJsonAsync($"reporterForm/", "");
            var json = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}