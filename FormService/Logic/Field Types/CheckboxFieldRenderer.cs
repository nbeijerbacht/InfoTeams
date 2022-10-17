using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic.Field_Types
{
    public class CheckboxFieldRenderer : IElementRenderer
    {
        public bool CanHandle(Element e) => e is { element_type: "field", field.type: "checkbox" };

        public IEnumerable<AdaptiveElement> RenderElements(Element e)
        {
            yield return new AdaptiveToggleInput
            {
                Id = e.field.field_id.ToString(),
                Title = e.field.name,
                Label = e.field.description,
                Value = e.field.default_value?.ToString()
            };
        }
    }
}
