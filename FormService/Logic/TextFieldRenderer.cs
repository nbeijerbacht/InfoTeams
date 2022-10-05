using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic;

public class TextFieldRenderer : IElementRenderer
{
    public bool CanHandle(Element e) => e is { element_type: "field", field.type: "text" };

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        yield return new AdaptiveTextBlock
        {
            Text = e.text
        };
        yield return new AdaptiveTextInput() { Id = e.element_id.ToString() };
    }
}
