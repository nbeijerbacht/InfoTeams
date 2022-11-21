using FormService.DTO;

namespace FormService.Logic;

public interface IFieldHandler
{
    bool CanHandle(string type);

    FieldOutput? Handle(FieldInput field);
}
