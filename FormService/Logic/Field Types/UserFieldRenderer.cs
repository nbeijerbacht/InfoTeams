using AdaptiveCards;
using FormService.DTO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace FormService.Logic;

public class UserFieldRenderer : IElementRenderer, ILookupFieldChoiceSearch
{

    private readonly IHttpClientFactory _httpClientFactory;

    public UserFieldRenderer(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }


    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "user" };

    public async Task<IEnumerable<AdaptiveChoice>> GetChoices(Element element, string query)
    {
        var client =  _httpClientFactory.CreateClient();

        var responseUser = await client.GetAsync($"https://localhost:7071/lookupinformation/search_users?search={query}");
        var responseTeam = await client.GetAsync($"https://localhost:7071/lookupinformation/search_teams?search={query}");

        var jsonUser = await responseUser.Content.ReadAsStringAsync();
        var jsonTeam = await responseTeam.Content.ReadAsStringAsync();

        var parseUser = Regex.Replace(jsonUser, string.Format(@"\b{0}\b", "user_id"), "id");
        var parseTeam = Regex.Replace(jsonTeam, string.Format(@"\b{0}\b", "team_id"), "id");

        var users = JsonConvert.DeserializeObject<List<UserAndTeamLookupDTO>>(parseUser);
        var teams = JsonConvert.DeserializeObject<List<UserAndTeamLookupDTO>>(parseTeam);

        var combinedList = users.Union(teams);

        combinedList = combinedList.OrderBy(value => value.name);

        return combinedList.Select(v => new AdaptiveChoice()
        {
            Title = v.name,
            Value = v.id,
        });
    }

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        yield return new AdaptiveTextInput()
        {
            Label = e.text,
            Id = e.field.field_id.ToString() + "-search",
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
            //Value = e.field.default_value?.ToString(),
            //Choices = e.field.list_items?.Select(item =>
            //    new AdaptiveChoice
            //    {
            //        Title = item.name,
            //        Value = item.list_item_id.ToString(),
            //    }
            //)?.ToList()
        };
    }
}
