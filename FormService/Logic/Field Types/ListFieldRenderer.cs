using AdaptiveCards;
using FormService.DTO;
using Newtonsoft.Json.Linq;

namespace FormService.Logic;

public class ListFieldRenderer : IElementRenderer
{
    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "list" or "numeric_list" };

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        yield return new AdaptiveChoiceSetInput
        {
            Id = e.field.field_id.ToString(),
            Label = e.text,
            Value = e.field.default_value?.ToString(),
            Choices = e.field.list_items?.Select(item =>
                new AdaptiveChoice
                {
                    Title = item.name,
                    Value = item.list_item_id.ToString(),
                }
            )?.ToList()
        };
    }
}
