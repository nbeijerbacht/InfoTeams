using FormService.DTO;

namespace FormService.Logic.FieldSerializers;

public class ListOfStringsHandler : IFieldHandler
{
    public bool CanHandle(string type) => type is "user" or "position";

    public FieldOutput Handle(FieldInput field) => new FieldOutput(field)
    {
        value = new[] { field.value.ToString() }
    };
}
