using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic;

public interface IAdaptiveCardRenderer
{
    AdaptiveCard Render(ReportFormDTO formData);
}
