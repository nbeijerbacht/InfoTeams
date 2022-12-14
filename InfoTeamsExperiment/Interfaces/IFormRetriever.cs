using AdaptiveCards;

namespace ZenyaBot.Interfaces;

public interface IFormRetriever
{
    Task<AdaptiveCard> GetCardByFormId(string formId, string lookUpField = "", string lookUpQuery= "");
}
