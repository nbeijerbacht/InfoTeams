using Microsoft.Bot.Builder;

namespace ZenyaBot.Interfaces;

public interface ITeamsMessageHandler
{
    bool CanHandle(MessageType type);

    Task Handle(ITurnContext turnContext, IDictionary<string, object> messageData, CancellationToken cancellation = default);
}

public enum MessageType
{
    SelectForm,
    SubmitForm,
}
