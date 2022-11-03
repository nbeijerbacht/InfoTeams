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
        var adaptiveCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
        {
            Body = formData.design.elements.SelectMany(this.ParseElement).ToList(),
        };

        return adaptiveCard;
    }

    private IEnumerable<AdaptiveElement> ParseElement(Element e)
    {
        var render = renderers.FirstOrDefault(r => r.CanHandle(e)) ?? throw new CannotRenderElement(e);
        return render.RenderElements(e);
    }
}
