using AdaptiveCards;
using FormService.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FormService.Controllers;

[ApiController]
[Route("[controller]")]
public class FormController : ControllerBase
{
    private readonly ILogger<FormController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public FormController(ILogger<FormController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet(Name = "SearchThroughForms")]
    public async Task<SearchResultDTO> Search([FromQuery(Name = "search")] string search)
    {
        var client = this._httpClientFactory.CreateClient();

        var response = await client.GetAsync($"https://localhost:7071/reporterForm?search={search}");
        var json = await response.Content.ReadAsStringAsync();
        var formData = JsonConvert.DeserializeObject<List<ReportFormDTO>>(json);

        var adaptiveCards = formData.Select(f => new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
        {
            Body =
            {
                new AdaptiveTextBlock
                {
                    Text = f.title,
                    Size = AdaptiveTextSize.Large,
                },
                new AdaptiveTextBlock
                {
                    Text = f.description,
                    Size = AdaptiveTextSize.Medium,
                },
            }
        }.ToJson());

        return new SearchResultDTO
        {
            Results = adaptiveCards,
        };

    }

    [HttpGet(Name = "FormById")]
    [Route("{form_id:int}")]
    public async Task<ReportFormDTO> GetFormById(int form_id)
    {
        var client = this._httpClientFactory.CreateClient();

        var response = await client.GetAsync($"https://localhost:7071/reporterForm/{form_id}");

        var json = await response.Content.ReadAsStringAsync();

        _logger.LogInformation(json);

        return new ReportFormDTO();
    }
}