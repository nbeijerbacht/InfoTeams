using AdaptiveCards;

namespace ZenyaBot.Interfaces;

public interface IFormFiller
{
    void FillInFormValues(IEnumerable<AdaptiveElement> elements, IDictionary<string, object> data);
}
