using FormService.DTO;

namespace FormService.Logic;

public class ListOfStringsHandler : IFieldHandler
{
    public bool CanHandle(string type) => type is "user";

    public FieldOutput Handle(FieldInput field) => new FieldOutput(field)
    {
        value = new[] { field.value.ToString() }
    };
}
