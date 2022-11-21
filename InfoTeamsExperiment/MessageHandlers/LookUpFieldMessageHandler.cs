using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
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
        var formId = messageData["form_id"].ToString();

        var lookUpId = messageData["id"].ToString()!;

        if (messageData.TryGetValue(lookUpId + "-search", out object? lookupQuery) is false)
            lookupQuery = "";

        var card = await formRetriever.GetCardByFormId(
            formId!,
            lookUpField: lookUpId,
            lookUpQuery: lookupQuery?.ToString() ?? ""
            );
        formFiller.FillInFormValues(card.Body, messageData);

        var attachment = new Attachment
        {
            ContentType = AdaptiveCard.ContentType,
            Content = card,
        };
        var activity = MessageFactory.Attachment(attachment);
        activity.Id = turnContext.Activity.ReplyToId;

        await turnContext.UpdateActivityAsync(activity, cancellation);
    }
}
