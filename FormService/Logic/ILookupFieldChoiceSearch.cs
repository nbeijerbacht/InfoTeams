using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic;

public interface ILookupFieldChoiceSearch
{
    bool CanHandle(Element element);

    Task<IEnumerable<AdaptiveChoice>> GetChoices(Element element, string? query);
}
