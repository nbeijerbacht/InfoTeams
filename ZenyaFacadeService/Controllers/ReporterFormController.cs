using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
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

        [HttpGet(Name = "SearchThroughForms")]
        public async Task<IEnumerable<FormDTO>> SearchThroughForms([FromQuery(Name = "search")] string search)
        {
            var client = _httpClientFactory.CreateClient();
            var path = "https://msteams.zenya.work/api/cases/reporter_forms";

            var response = await client.GetAsync(path);

            var json = await response.Content.ReadAsStringAsync();
            var form_data = JsonConvert.DeserializeObject<List<FormDTO>>(json);

            return form_data.Where(f => f.title.Contains(search) || f.description.Contains(search));
        }
    }
}