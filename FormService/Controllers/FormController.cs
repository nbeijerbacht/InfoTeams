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
    private readonly IAdaptiveCardRenderer renderer;
    private readonly IEnumerable<IFieldHandler> fieldHandlers;

    public FormController(
        ILogger<FormController> logger,
        IHttpClientFactory httpClientFactory,
        IAdaptiveCardRenderer rederer,
        IEnumerable<IFieldHandler> fieldHandlers)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        this.renderer = rederer;
        this.fieldHandlers = fieldHandlers;
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
            Id = f.form_id.ToString(),
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

        if (json is not "Not found")
        {
            var formData = JsonConvert.DeserializeObject<ReportFormDTO>(json);

            var result = renderer.Render(formData);

            return new FormResultDTO
            {
                Result = result.ToJson(),
            };
        }
        else return new FormResultDTO { Result = "Oops something went wrong" };
    }

    [HttpPost]
    public async Task SubmitForm(FormInput formInput)
    {
        var client = this._httpClientFactory.CreateClient();

        var formResponse = await client.GetAsync($"https://localhost:7071/reporterForm/{formInput.form_id}");
        var json = await formResponse.Content.ReadAsStringAsync();
        var formData = JsonConvert.DeserializeObject<ReportFormDTO>(json);

        var formOut = new FormOutputDTO
        {
            form_id = formInput.form_id,
            fields = new List<FieldOutput>(),
        };

        foreach (var inputField in formInput.fields)
        {
            var fieldDefinition = formData.design.elements
                .Select(element => element.field)
                .First(field => field?.field_id.ToString() == inputField.field_id);

            var output = this.fieldHandlers
                .First(h => h.CanHandle(fieldDefinition.type))
                .Handle(inputField);

            formOut.fields.Add(output);
        }

        var postResponse = await client.PostAsJsonAsync($"https://localhost:7071/reporterForm/", formOut);

        Response.StatusCode = (int)postResponse.StatusCode;
        Response.ContentType = "application/json; charset=utf-8";

        await Response.WriteAsync(await postResponse.Content.ReadAsStringAsync());
        await Response.CompleteAsync();
    }
}