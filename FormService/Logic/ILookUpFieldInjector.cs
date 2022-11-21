using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic;

public interface ILookUpFieldInjector
{
    Task InjectChoices(AdaptiveCard card, ReportFormDTO formData, int fieldId, string? query);
}
