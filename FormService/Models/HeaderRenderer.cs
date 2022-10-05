using AdaptiveCards;
using FormService.DTO;

namespace FormService.Models;

public class HeaderRenderer : IElementRenderer
{
    private static Dictionary<string, AdaptiveTextSize> headerTypes = new()
    {
        ["header"] = AdaptiveTextSize.Large,
        ["sub_header"] = AdaptiveTextSize.Medium,
        ["description"] = AdaptiveTextSize.Default,
    };

    public bool CanHandle(Element e) => headerTypes.ContainsKey(e.element_type);

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        yield return new AdaptiveTextBlock
        {
            Text = e.text,
            Size = headerTypes[e.element_type],
        };
    }
}
