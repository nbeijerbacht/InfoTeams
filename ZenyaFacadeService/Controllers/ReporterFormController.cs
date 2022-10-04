using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var client = _httpClientFactory.CreateClient();
            var path = "https://msteams.zenya.work/api/cases/reporter_forms";

            var response = await client.GetAsync(path);

            var json = await response.Content.ReadAsStringAsync();
            var form_data = JsonConvert.DeserializeObject<List<FormDTO>>(json);

            return form_data.Where(f => f.title.Contains(search) || f.description.Contains(search));
        }

        [HttpGet]
        [Route("{form_id:int}")]
        public async Task<FormDTO> FormById(int form_id)
        {
            var client = _httpClientFactory.CreateClient();
            var path = $"https://msteams.zenya.work/api/cases/reporter_forms/{form_id}?include_design=true";

            var response = await client.GetAsync(path);

            var json = await response.Content.ReadAsStringAsync();
            var form_data = JsonConvert.DeserializeObject<FormDTO>(json);

            return form_data;
        }
    }
}