using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic.ElementRenderers;

public class IgnoreRenderer : IElementRenderer
{
    private static string[] _ignoreElementTypes = new string[]
    {
        "page_break", "horizontal_line"
    };
    private static string[] _ignoreFieldTypes = new string[]
    {
    };
    public bool CanHandle(Element e) => _ignoreElementTypes.Contains(e.element_type) || _ignoreFieldTypes.Contains(e.field?.type);

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        return Array.Empty<AdaptiveElement>();
    }
}
