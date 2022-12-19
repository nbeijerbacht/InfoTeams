using AdaptiveCards;
using FormService.DTO;
using Newtonsoft.Json.Linq;

namespace FormService.Logic.ElementRenderers.Fields;

public class SubjectTreeFieldRenderer : IElementRenderer
{
    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "subject_tree" };

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        var @default = e.field.default_value?["subject_id"]?.ToString();
        yield return new AdaptiveTextInput()
        {
            Id = e.field.field_id.ToString(),
            Label = e.text,
            Value = @default,
            IsVisible = false,
        };
    }
}
