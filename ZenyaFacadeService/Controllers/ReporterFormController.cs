using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ZenyaFacadeService.DTO;
using ZenyaFacadeService.HttpClient;

namespace ZenyaFacadeService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReporterFormController : ControllerBase
    {
        private readonly ILogger<ReporterFormController> _logger;
        private readonly IZenyaHttpClient _client;

        public ReporterFormController(ILogger<ReporterFormController> logger, IZenyaHttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public async Task<IEnumerable<FormDTO>> SearchThroughForms([FromQuery(Name = "search")] string search)
        {
            var json = await _client.GetAllForms();
            var form_data = JsonConvert.DeserializeObject<List<FormDTO>>(json);

            var search_lower = search.ToLowerInvariant();
            return form_data.Where(f => f.title.ToLowerInvariant().Contains(search_lower) ||
                                        f.description.ToLowerInvariant().Contains(search_lower));
        }

        [HttpGet]
        [Route("{form_id:int}")]
        public async Task<string> FormById(int form_id)
        {
            var json = await _client.GetFormById(form_id);
            if (json is not null) return await _client.GetFormById(form_id);
            else return "Not found";
        }

        [HttpPost]
        public async Task SubmitForm([FromBody] JsonElement body)
        {
            var response = await _client.PostForm(body);

            Response.StatusCode = (int) response.StatusCode;
            Response.ContentType = "application/json; charset=utf-8";

            await Response.WriteAsync(await response.Content.ReadAsStringAsync());
            await Response.CompleteAsync();
        }
    }
}