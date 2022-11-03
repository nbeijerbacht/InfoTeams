using FormService.DTO;

namespace FormService.Logic;


public class BooleanHandler : IFieldHandler
{
    public bool CanHandle(string type) => type is "checkbox";

    public FieldOutput Handle(FieldInput field) => new(field)
    {
        value = bool.Parse(field.value.ToString()),
    };
}
