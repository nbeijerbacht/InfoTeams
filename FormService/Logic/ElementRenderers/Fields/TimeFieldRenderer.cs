using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic.ElementRenderers.Fields;

public class TimeFieldRenderer : IElementRenderer
{
    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "time" };

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        if (e.field.read_only)
            yield return new AdaptiveTextBlock
            {
                Text = e.text + " " + GetDefaultTime(e.field),
            };
        else 
            yield return new AdaptiveTimeInput() 
            {
                Id = e.field.field_id.ToString(),
                Label = e.text,
                Value = GetDefaultTime(e.field),
            };
    }

    private static string GetDefaultTime(Field field)
    {
        if (field.default_value?.ToString() == "current_time")
            return DateTime.Now.ToShortTimeString();
        else
            return "";
    }
}
