using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ZenyaFacadeService.DTO;

namespace ZenyaFacadeService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReporterFormController : ControllerBase
    {
        private readonly ILogger<ReporterFormController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ReporterFormController(ILogger<ReporterFormController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IEnumerable<FormDTO>> SearchThroughForms([FromQuery(Name = "search")] string search)
        {
            var client = _httpClientFactory.CreateClient("ZenyaClient");
            var path = "https://msteams.zenya.work/api/cases/reporter_forms";

            var response = await client.GetAsync(path);

            var json = await response.Content.ReadAsStringAsync();
            var form_data = JsonConvert.DeserializeObject<List<FormDTO>>(json);

            var search_lower = search.ToLowerInvariant();
            return form_data.Where(f => f.title.ToLowerInvariant().Contains(search_lower) ||
                                        f.description.ToLowerInvariant().Contains(search_lower));
        }

        [HttpGet]
        [Route("{form_id:int}")]
        public async Task<string> FormById(int form_id)
        {
            var client = _httpClientFactory.CreateClient("ZenyaClient");
            var path = $"https://msteams.zenya.work/api/cases/reporter_forms/{form_id}?include_design=true";

            var response = await client.GetAsync(path);

            var json = await response.Content.ReadAsStringAsync();

            return json;
        }

        [HttpPost]
        public async Task SubmitForm([FromBody] JsonElement body)
        {
            var client = _httpClientFactory.CreateClient("ZenyaClient");
            var path = $"https://msteams.zenya.work/api/cases";

            var response = await client.PostAsJsonAsync(path, body);

            Response.StatusCode = (int) response.StatusCode;
            Response.ContentType = "application/json; charset=utf-8";

            await Response.WriteAsync(await response.Content.ReadAsStringAsync());
            await Response.CompleteAsync();
        }
    }
}