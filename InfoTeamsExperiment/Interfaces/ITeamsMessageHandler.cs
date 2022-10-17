using Microsoft.Bot.Builder;

namespace ZenyaBot.Interfaces;

/// <summary>
/// This interface is for handling textless messages called actions.
/// Pressing a button on an adaptive card is a kind of action. 
/// </summary>
public interface ITeamsActionHandler
{
    /// <summary>
    /// Test if this handler can handle a specific custom type of action.
    /// </summary>
    /// <param name="type">The type of action. E.g. Fill-in button versus Submit button.</param>
    /// <returns>True if the handler can handle this action type.</returns>
    bool CanHandle(CustomActionType type);

    /// <summary>
    /// Handle the action. Can do asynchronous work and optionally reply to the user.
    /// </summary>
    /// <param name="turnContext">Allows the handler to send messages to the conversatio tha ttriggered the action.</param>
    /// <param name="messageData">Custom Json Data sent along with the Action. E.g. form values.</param>
    /// <param name="cancellation">Cancellation token</param>
    /// <returns>A Task that completes after the handler is done handling the action.</returns>
    Task Handle(ITurnContext turnContext, IDictionary<string, object> messageData, CancellationToken cancellation = default);
}

public enum CustomActionType
{
    SelectForm,
    SubmitForm,
}
