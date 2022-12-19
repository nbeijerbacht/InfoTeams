using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic.ElementRenderers.Fields;

public class DateFieldRenderer : IElementRenderer
{
    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "date" };

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        var default_value = e.field.default_value?.ToString();
        yield return new AdaptiveDateInput() 
        { 
            Id = e.field.field_id.ToString(),
            Label = e.text,
            Value =  default_value == "current_date" ? DateTime.Now.ToString("yyyy-MM-dd") : "" ,
        };
    }
}
