using AdaptiveCards;
using FormService.DTO;
using Newtonsoft.Json;

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

        var response = await client.GetAsync($"https://localhost:7071/lookupinformation/search_users?search={query}");

        var json = await response.Content.ReadAsStringAsync();

        var users = JsonConvert.DeserializeObject<List<UserLookupDTO>>(json);

        List<AdaptiveChoice> choices = new List<AdaptiveChoice>();

        foreach (UserLookupDTO user in users)
        {
            var choice = new AdaptiveChoice() { 
                Title = user.name,
                Value = user.user_id,
            };

            choices.Add(choice);
        }

        return choices;
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
