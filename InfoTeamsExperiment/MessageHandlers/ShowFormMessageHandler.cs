using AdaptiveCards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Net;
using ZenyaBot.DTO;
using ZenyaBot.Interfaces;

namespace ZenyaBot.MessageHandlers;

/// <inheritdoc />
public class ShowFormActionHandler : ITeamsActionHandler
{
    private readonly ILogger<ShowFormActionHandler> logger;
    private readonly IHttpClientFactory clientFactory;

    public ShowFormActionHandler(ILogger<ShowFormActionHandler> logger, IHttpClientFactory clientFactory)
    {
        this.logger = logger;
        this.clientFactory = clientFactory;
    }

    /// <inheritdoc />
    public bool CanHandle(CustomActionType type) => type == CustomActionType.SelectForm;

    /// <inheritdoc />
    public async Task Handle(ITurnContext turnContext, IDictionary<string, object> messageData, CancellationToken cancellation = default)
    {
        var form_id = messageData["form_id"]?.ToString();
        
        var client = this.clientFactory.CreateClient();
        var path = "https://localhost:7072/form/" + form_id;
        var response = await client.GetAsync(path, cancellation);
        
        var json = await response.Content.ReadAsStringAsync(cancellation);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            await turnContext.SendActivityAsync("No form was found please try again.");
        }

        var result = JsonConvert.DeserializeObject<FormResultDTO>(json).Result;

        var parsedCard = AdaptiveCard.FromJson(result);

        foreach (var warning in parsedCard.Warnings)
        {
            this.logger.LogWarning($"Warning {(AdaptiveWarning.WarningStatusCode) warning.Code}" +
                $"received when parsing Adaptive Card {warning.Message}");
        }

        var card = parsedCard.Card;

        card.Actions.Add(new AdaptiveSubmitAction
        {
            Title = "Submit",
            DataJson = JsonConvert.SerializeObject(new
            {
                type = CustomActionType.SubmitForm.ToString(),
                form_id,
            }),
        });

        var attachment = MessageFactory.Attachment(new Attachment
        {
            ContentType = AdaptiveCard.ContentType,
            Content = card,
        });

        await turnContext.SendActivityAsync(attachment, cancellation);
    }
}
