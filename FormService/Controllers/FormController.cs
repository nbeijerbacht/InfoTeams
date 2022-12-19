using AdaptiveCards;
using FormService.DTO;
using FormService.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Newtonsoft.Json;
using System.Net;

namespace FormService.Controllers;

[ApiController]
[Route("[controller]")]
public class FormController : ControllerBase
{
    private readonly IConfiguration config;
    private readonly ILogger<FormController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAdaptiveCardRenderer renderer;
    private readonly ILookUpFieldInjector lookupHandler;
    private readonly IEnumerable<IFieldHandler> fieldHandlers;

    public FormController(
        IConfiguration config,
        ILogger<FormController> logger,
        IHttpClientFactory httpClientFactory,
        IAdaptiveCardRenderer rederer,
        ILookUpFieldInjector lookupHandler,
        IEnumerable<IFieldHandler> fieldHandlers)
    {
        this.config = config;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        this.renderer = rederer;
        this.lookupHandler = lookupHandler;
        this.fieldHandlers = fieldHandlers;
    }

    private string FacadeUrl => this.config.GetSection("Services")["FacadeService"];

    [HttpGet]
    public async Task<SearchResultDTO> Search([FromQuery(Name = "search")] string search)
    {
        var client = this._httpClientFactory.CreateClient();
        var path = $"{FacadeUrl}reporterForm?search={search}";

        var response = await client.GetAsync(path);
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
    public async Task<ActionResult<FormResultDTO>> GetFormById(
        int form_id,
        int? lookupFieldId = null,
        string? lookupFieldQuery = null)
    {
        var client = this._httpClientFactory.CreateClient();

        var response = await client.GetAsync($"{FacadeUrl}reporterForm/{form_id}");

        if (response.StatusCode == HttpStatusCode.NotFound) return this.NotFound();

        var json = await response.Content.ReadAsStringAsync();

        var formData = JsonConvert.DeserializeObject<ReportFormDTO>(json);

        var result = renderer.Render(formData);

        if (lookupFieldId.HasValue)
            await lookupHandler.InjectChoices(result, formData, lookupFieldId.Value, lookupFieldQuery);

        return this.Ok(new FormResultDTO
        {
            Result = result.ToJson(),
        });
    }

    [HttpPost]
    public async Task SubmitForm(FormInput formInput)
    {
        var client = this._httpClientFactory.CreateClient();

        var formResponse = await client.GetAsync($"{FacadeUrl}reporterForm/{formInput.form_id}");
        var json = await formResponse.Content.ReadAsStringAsync();
        var formData = JsonConvert.DeserializeObject<ReportFormDTO>(json);

        var formOut = new FormOutputDTO
        {
            form_id = formInput.form_id,
            fields = new List<FieldOutput>(),
            is_draft = formInput.is_draft,
        };

        foreach (var inputField in formInput.fields)
        {
            var fieldDefinition = formData.design.elements
                .Select(element => element.field)
                .FirstOrDefault(field => field?.field_id.ToString() == inputField.field_id)
                ?? throw new InvalidOperationException("Could not handle field:" + JsonConvert.SerializeObject(inputField));

            var handler = this.fieldHandlers
                .FirstOrDefault(h => h.CanHandle(fieldDefinition.type))
                ?? throw new InvalidOperationException("Could not handle field:" + JsonConvert.SerializeObject(fieldDefinition));
                
             var output = handler.Handle(inputField);

            if (output != null) formOut.fields.Add(output);
        }

        var postResponse = await client.PostAsJsonAsync($"{FacadeUrl}reporterForm/", formOut);

        Response.StatusCode = (int)postResponse.StatusCode;
        Response.ContentType = "application/json; charset=utf-8";

        await Response.WriteAsync(await postResponse.Content.ReadAsStringAsync());
        await Response.CompleteAsync();
    }
}