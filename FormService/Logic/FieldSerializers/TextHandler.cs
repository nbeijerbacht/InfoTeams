using FormService.DTO;

namespace FormService.Logic.FieldSerializers;

public class TextHandler : IFieldHandler
{
    public bool CanHandle(string type) => type is "text" or "formatted_text" or "email" or "web_page" or "time";

    public FieldOutput Handle(FieldInput field) => new FieldOutput(field);
}
