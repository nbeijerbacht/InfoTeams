using FormService.DTO;

namespace FormService.Logic;

public class NumericHandler : IFieldHandler
{
    private static string[] types = new[] { "numeric", "list", "subject_tree" };

    public bool CanHandle(string type) => types.Contains(type);

    public FieldOutput Handle(FieldInput field) => new FieldOutput(field)
    {
        value = int.Parse(field.value.ToString()),
    };
}
