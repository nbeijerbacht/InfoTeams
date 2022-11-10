using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic;

public class UserFieldRenderer : IElementRenderer, ILookupFieldChoiceSearch
{
    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "user" };

    public async Task<IEnumerable<AdaptiveChoice>> GetChoices(Element element, string query)
    {
        return new List<AdaptiveChoice>
        {
            new ()
            {
                Title = "John Doe",
                Value = "1",
            },
            new ()
            {
                Title = "Jane Doe",
                Value = "2",
            },
        };
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
