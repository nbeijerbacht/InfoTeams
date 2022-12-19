using AdaptiveCards;
using FormService.DTO;
using Newtonsoft.Json;

namespace FormService.Logic;

public class UnhandlebleFieldRenderer : IElementRenderer
{

    public bool CanHandle(Element e) => false;

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        yield return new AdaptiveTextBlock()
        {
           Color = AdaptiveTextColor.Warning,
           Text = $"{e.text}: Cannot render {e?.field?.type} {e.element_type}",
           Id = e?.field?.field_id.ToString() ?? e.element_id.ToString(),
        };
    }
}
