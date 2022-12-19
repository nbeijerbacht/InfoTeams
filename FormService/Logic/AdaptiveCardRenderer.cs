using AdaptiveCards;
using FormService.DTO;
using FormService.Exceptions;

namespace FormService.Logic;

public class AdaptiveCardRenderer : IAdaptiveCardRenderer
{
    private readonly IEnumerable<IElementRenderer> renderers;

    public AdaptiveCardRenderer(IEnumerable<IElementRenderer> renderers)
    {
        this.renderers = renderers;
    }
    public AdaptiveCard Render(ReportFormDTO formData)
    {
        var adaptiveCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 4))
        {
            Body = formData.design.elements.SelectMany(this.ParseElement).ToList(),
            Id = formData.form_id.ToString(),
        };

        foreach (var a in formData.design.elements)
            if (a?.field?.required == true)
            {
                var element = adaptiveCard.Body
                    .Where(e => e.Id == a?.field?.field_id.ToString())
                    .FirstOrDefault();

                if (element is AdaptiveInput inp)
                    inp.IsRequired = true;
            }

        adaptiveCard.Body.Add(new AdaptiveTextInput
        {
            IsVisible = false,
            Value = formData.form_id.ToString(),
            Id = "form_id",
        });

        return adaptiveCard;
    }

    private IEnumerable<AdaptiveElement> ParseElement(Element e)
    {
        var render = renderers
            .FirstOrDefault(r => r.CanHandle(e)) 
            ?? new UnhandlebleFieldRenderer();

        return render.RenderElements(e);
    }
}
