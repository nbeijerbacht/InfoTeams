using FormService.DTO;
using Newtonsoft.Json;

namespace FormService.Exceptions
{
    public class CannotRenderElement : Exception
    {
        public CannotRenderElement(Element e) : base ($"Cannot render element: \n{JsonConvert.SerializeObject(e)}")
        {
        }
    }
}
