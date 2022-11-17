using AdaptiveCards;
using ZenyaBot.Interfaces;

namespace ZenyaBot.Logic
{
    public class FormFiller : IFormFiller
    {
        public void FillInFormValues(IEnumerable<AdaptiveElement> elements, IDictionary<string, object> data)
        {
            foreach (var (key, value) in data)
            {
                var element = elements
                    .Where(e => e.Id == key)
                    .FirstOrDefault();

                if (element is null)
                {
                    // throw only for integer keys
                    if (int.TryParse(key, out int _))
                        throw new InvalidOperationException($"Could not find element with id {key} in form");
                    else
                        continue;
                }
                    
                

                var val = value.ToString()!;

                switch (element)
                {
                    case AdaptiveTextInput input:
                        input.Value = val;
                        break;
                    case AdaptiveChoiceSetInput input:
                        input.Value = val;
                        break;
                    case AdaptiveNumberInput input:
                        input.Value = double.Parse(val);
                        break;
                    case AdaptiveDateInput input:
                        input.Value = val;
                        break;
                    case AdaptiveTimeInput input:
                        input.Value = val;
                        break;
                    case AdaptiveToggleInput input:
                        input.Value = val;
                        break;
                    default:
                        throw new InvalidOperationException($"Element {element} is not a supported Input");
                }
            }
        }   
    }
}
