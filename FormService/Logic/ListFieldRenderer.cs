using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic;

public class ListFieldRenderer : IElementRenderer
{
    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "list" };

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
            yield return new AdaptiveChoiceSetInput
        {
            Id = e.element_id.ToString(),
            Label = e.text,
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
