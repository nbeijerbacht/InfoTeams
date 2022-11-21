using AdaptiveCards;
using FormService.DTO;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace FormService.Logic;

public class LookupFieldInjector : ILookUpFieldInjector
{
    private readonly IEnumerable<ILookupFieldChoiceSearch> lookupHandlers;

    public LookupFieldInjector(IEnumerable<ILookupFieldChoiceSearch> lookupHandlers)
    {
        this.lookupHandlers = lookupHandlers;
    }

    public async Task InjectChoices(AdaptiveCard card, ReportFormDTO formData, int fieldId, string? query)
    {
        var definition = formData.design.elements
            .Where(e => e.field?.field_id == fieldId)
            .SingleOrDefault()
            ?? throw new ArgumentException(
                $"Could not find field with id={fieldId} in form {JsonConvert.SerializeObject(formData)}", 
               nameof(fieldId));

        var element = card.Body.Where(e => e.Id == fieldId.ToString()).SingleOrDefault();

        if (element is not AdaptiveChoiceSetInput dropdown)
            throw new ArgumentException($"{element} is not an AdaptiveChoiceSetInput", nameof(fieldId));

        var handler = lookupHandlers
            .FirstOrDefault(h => h.CanHandle(definition))
            ?? throw new InvalidOperationException($"Could not find handler for field {JsonConvert.SerializeObject(definition)}");

        dropdown.Choices.AddRange(await handler.GetChoices(definition, query));
    }
}
