using Microsoft.Bot.Builder;
using System.Threading;
using ZenyaBot.DTO;
using ZenyaBot.Interfaces;

namespace ZenyaBot.MessageHandlers;

/// <inheritdoc />
public class LookUpFieldMessageHandler : ITeamsActionHandler
{
    private readonly IFormFiller formFiller;
    private readonly IFormRetriever formRetriever;

    public LookUpFieldMessageHandler(
        IFormFiller formFiller,
        IFormRetriever formRetriever)
    {
        this.formFiller = formFiller;
        this.formRetriever = formRetriever;
    }

    /// <inheritdoc />
    public bool CanHandle(CustomActionType type) => type == CustomActionType.LookUpField;

    /// <inheritdoc />
    public async Task Handle(ITurnContext turnContext, IDictionary<string, object> messageData, CancellationToken cancellation = default)
    {
        var newActivity = MessageFactory.Text("The new text for the activity");
        newActivity.Id = turnContext.Activity.ReplyToId;

        var formId = messageData["form_id"].ToString();
        var card = await formRetriever.GetCardByFormId(formId!);
        formFiller.FillInFormValues(card.Body, messageData);

        await turnContext.UpdateActivityAsync(newActivity, cancellation);
    }
}
