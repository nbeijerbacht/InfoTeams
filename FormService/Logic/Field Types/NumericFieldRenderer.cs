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
            Id = e.field.field_id.ToString(),
            Label = e.text,
            ErrorMessage = "This field must be a number ",
        };

        var (min, max) = (e.field.min_numeric_value, e.field.max_numeric_value);
        if (min != null)
        {
            number.Min = min.Value;
            number.ErrorMessage += $"bigger than {min} ";
        }

        if (max != null)
        {
            number.Max = max.Value;
            number.ErrorMessage += $"and smaller than {max} ";
        }

        if (e.field.max_numeric_value != null)

        yield return number;
    }
}
