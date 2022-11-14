using AdaptiveCards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Net;
using ZenyaBot.DTO;
using ZenyaBot.Exceptions;
using ZenyaBot.Interfaces;

namespace ZenyaBot.MessageHandlers;

/// <inheritdoc />
public class ShowFormActionHandler : ITeamsActionHandler
{
    private readonly ILogger<ShowFormActionHandler> logger;
    private readonly IFormRetriever formRetriever;

    public ShowFormActionHandler(ILogger<ShowFormActionHandler> logger, IFormRetriever formRetriever)
    {
        this.logger = logger;
        this.formRetriever = formRetriever;
    }

    /// <inheritdoc />
    public bool CanHandle(CustomActionType type) => type == CustomActionType.SelectForm;

    /// <inheritdoc />
    public async Task Handle(ITurnContext turnContext, IDictionary<string, object> messageData, CancellationToken cancellation = default)
    {
        var form_id = messageData["form_id"]?.ToString();
        AdaptiveCard card;
        try
        {
            card = await this.formRetriever.GetCardByFormId(form_id!);
        }
        catch (FormNotFound)
        {
            await turnContext.SendActivityAsync("No form was found please try again.");
            return;
        }

        var attachment = MessageFactory.Attachment(new Attachment
        {
            ContentType = AdaptiveCard.ContentType,
            Content = card,
        });

        await turnContext.SendActivityAsync(attachment, cancellation);
    }
}
