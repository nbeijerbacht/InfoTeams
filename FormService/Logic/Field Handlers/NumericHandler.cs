using FormService.DTO;

namespace FormService.Logic;

public class NumericHandler : IFieldHandler
{
    public bool CanHandle(string type) => type is "numeric" or "list" or "numeric_list" or "subject_tree";

    public FieldOutput Handle(FieldInput field) => new(field)
    {
        value = int.Parse(field.value.ToString()),
    };
}