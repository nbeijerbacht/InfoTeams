using AdaptiveCards;
using FormService.DTO;
using FormService.Logic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FormService.Controllers;

[ApiController]
[Route("[controller]")]
public class FormController : ControllerBase
{
    private readonly ILogger<FormController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAdaptiveCardRenderer rederer;

    public FormController(ILogger<FormController> logger, IHttpClientFactory httpClientFactory, IAdaptiveCardRenderer rederer)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        this.rederer = rederer;
    }

    [HttpGet]
    public async Task<SearchResultDTO> Search([FromQuery(Name = "search")] string search)
    {
        var client = this._httpClientFactory.CreateClient();

        var response = await client.GetAsync($"https://localhost:7071/reporterForm?search={search}");
        var json = await response.Content.ReadAsStringAsync();
        var formData = JsonConvert.DeserializeObject<List<ReportFormDTO>>(json);

        var adaptiveCards = formData.Select(f => new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
        {
            Id = f.form_id?.ToString(),
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

    [HttpGet]
    [Route("{form_id:int}")]
    public async Task<FormResultDTO> GetFormById(int form_id)
    {
        var client = this._httpClientFactory.CreateClient();

        var response = await client.GetAsync($"https://localhost:7071/reporterForm/{form_id}");

        var json = await response.Content.ReadAsStringAsync();

        var formData = JsonConvert.DeserializeObject<ReportFormDTO>(json);

        var result = rederer.Render(formData).ToJson();

        _logger.LogInformation(result);

        return new FormResultDTO
        {
            Result = result,
        };
    }
}