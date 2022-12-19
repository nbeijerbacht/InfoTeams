using AdaptiveCards;
using FormService.DTO;
using Newtonsoft.Json;

namespace FormService.Logic.ElementRenderers.Fields;

public class UserFieldRenderer : IElementRenderer, ILookupFieldChoiceSearch
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration config;

    public UserFieldRenderer(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        this.config = config;
    }

    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "user" };

    private string FacadeServiceUrl => this.config.GetSection("Services")["FacadeService"];

    public async Task<IEnumerable<AdaptiveChoice>> GetChoices(Element element, string? query)
    {
        var client =  _httpClientFactory.CreateClient();

        var search = query is null ? "" : "search=" + query;

        var responseUserTask = client.GetAsync($"{FacadeServiceUrl}lookupinformation/search_users?{search}");
        var responseTeamTask = client.GetAsync($"{FacadeServiceUrl}lookupinformation/search_teams?{search}");

        var responseUser = await responseUserTask;
        var jsonUser = await responseUser.Content.ReadAsStringAsync();

        var responseTeam = await responseTeamTask;
        var jsonTeam = await responseTeam.Content.ReadAsStringAsync();

        var userChoices = JsonConvert
            .DeserializeObject<List<UserDTO>>(jsonUser)
            .Select(user => new AdaptiveChoice()
        {
            Title = user.name,
            Value = user.user_id,
        });

        var teamChoices = JsonConvert
            .DeserializeObject<List<TeamDTO>>(jsonTeam)
            .Select(team => new AdaptiveChoice()
        {
            Title = team.name,
            Value = team.team_id,
        });

        var combinedList = userChoices
            .Concat(teamChoices)
            .OrderBy(value => value.Title);

        return combinedList;
    }

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        yield return new AdaptiveTextInput()
        {
            Label = e.text,
            Id = e.field.field_id.ToString() + "-search",
            Placeholder = "Search for " + e.text,
            InlineAction = new AdaptiveSubmitAction()
            {
                Title = "Search",
                AssociatedInputs = AdaptiveAssociatedInputs.Auto,
                Id = e.field.field_id.ToString() + "-search-action",
                Data = new
                {
                    type = "LookUpField",
                    id = e.field.field_id,
                },
            },
        };
        yield return new AdaptiveChoiceSetInput
        {
            Id = e.field.field_id.ToString(),
            Label = "Choose " + e.text,
        };
    }
}
