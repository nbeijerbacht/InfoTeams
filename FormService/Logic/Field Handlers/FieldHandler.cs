using FormService.DTO;

namespace FormService.Logic
{
    public class TextHandler : IFieldHandler
    {
        private static string[] types = new[] { "text" };

        public bool CanHandle(string type) => types.Contains(type);

        public FieldOutput Handle(FieldInput field) => new FieldOutput(field);
    }
}
