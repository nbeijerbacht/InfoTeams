using AdaptiveCards;
using FormService.DTO;

namespace FormService.Logic;

public class TextFieldRenderer : IElementRenderer
{
    private Dictionary<string, (string? regex, string? message, string placeholder, string style)> properties = new()
    {
        ["text"] = (regex: null, message: null, placeholder: "", "text"),
        ["formatted_text"] = (regex: null, message: null, placeholder: "", "text"),

        ["email"] = (
            regex: @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            message: "Not a valid email",
            placeholder: "e.g., mail@example.com",
            style: "email"),

        ["web_page"] = (
            regex: @"[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)",
            message: "Not a valid URL",
            placeholder: "e.g., https://infoland.nl/",
            style: "url")
    };
    public bool CanHandle(Element e) => e is { 
        element_type: "field",
        field.type: "text" or "formatted_text" or "web_page" or "email" };

    public IEnumerable<AdaptiveElement> RenderElements(Element e)
    {
        if (e.field.read_only)
            yield return new AdaptiveTextBlock
            {
                Text = e.text,
            };
        else 
            yield return new AdaptiveTextInput() 
        {
            Id = e.field.field_id.ToString(),
            Label = e.text,
            Value = e.field.default_value?.ToString(),
            IsMultiline = e.field.text_lines > 1,
            AdditionalProperties = new SerializableDictionary<string, object> { ["tooltip"] = "lol"},
            Regex = properties[e.field.type].regex,
            ErrorMessage = properties[e.field.type].message,
            Placeholder = properties[e.field.type].placeholder,
        };
    }
}
