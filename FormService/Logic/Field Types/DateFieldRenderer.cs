using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic;

public class DateFieldRenderer : IElementRenderer
{
    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "date" };

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        yield return new AdaptiveDateInput() 
        { 
            Id = e.field.field_id.ToString(),
            Label = e.text,
        };
    }
}
