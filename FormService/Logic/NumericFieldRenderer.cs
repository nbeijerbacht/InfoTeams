using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic;

public class NumericFieldRenderer : IElementRenderer
{
    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "numeric" };

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        var number = new AdaptiveNumberInput() 
        { 
            Id = e.element_id.ToString(),
            Label = e.text,
        };

        if (e.field.min_numeric_value != null)
            number.Min = (double) e.field.min_numeric_value;

        if (e.field.max_numeric_value != null)
            number.Max = (double)e.field.max_numeric_value;

        yield return number;
    }
}
