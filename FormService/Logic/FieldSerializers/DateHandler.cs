using FormService.DTO;

namespace FormService.Logic.FieldSerializers;

public class DateHandler : IFieldHandler
{
    private static string[] types = new[] { "date" };

    public bool CanHandle(string type) => types.Contains(type);

    public FieldOutput Handle(FieldInput field) => new FieldOutput(field)
    {
        value = field.value.ToString().Replace("-", ""),
    };
}
