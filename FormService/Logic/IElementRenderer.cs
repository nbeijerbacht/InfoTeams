using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic;

public interface IElementRenderer
{
    bool CanHandle(Element e);

    IEnumerable<AdaptiveElement> RenderElements(Element e);
}
