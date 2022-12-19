using AdaptiveCards;
using FormService.DTO;
using Newtonsoft.Json;

namespace FormService.Logic.ElementRenderers.Fields;

public class PositionFieldRenderer : IElementRenderer
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration config;

    public PositionFieldRenderer(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        this.config = config;
    }

    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "position" };

    private string FacadeServiceUrl => this.config.GetSection("Services")["FacadeService"];

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        var client = _httpClientFactory.CreateClient();

        var response = client
            .GetAsync($"{FacadeServiceUrl}lookupinformation/get_positions")
            .GetAwaiter()
            .GetResult();

        var positions = response.Content
            .ReadAsStringAsync()
            .GetAwaiter()
            .GetResult();

        var choices = JsonConvert
            .DeserializeObject<List<PositionDTO>>(positions)
            .Select(user => new AdaptiveChoice()
            {
                Title = user.name,
                Value = user.id,
            });

        yield return new AdaptiveChoiceSetInput
        {
            Id = e.field.field_id.ToString(),
            Label = e.text,
            Value = e.field.default_value?.ToString(),
            Choices = choices.ToList(),
        };
    }
}
