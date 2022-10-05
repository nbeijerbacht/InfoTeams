using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using ZenyaBot.DTO;
using ZenyaBot.Interfaces;

namespace ZenyaBot.MessageHandlers;

public class ShowFormMessageHandler : ITeamsMessageHandler
{
    private readonly ILogger<ShowFormMessageHandler> logger;
    private readonly IHttpClientFactory clientFactory;

    public ShowFormMessageHandler(ILogger<ShowFormMessageHandler> logger, IHttpClientFactory clientFactory)
    {
        this.logger = logger;
        this.clientFactory = clientFactory;
    }

    public bool CanHandle(MessageType type) => type == MessageType.SelectForm;

    public async Task Handle(ITurnContext turnContext, IDictionary<string, object> messageData, CancellationToken cancellation = default)
    {
        var form_id = messageData["form_id"]?.ToString();
        
        var client = this.clientFactory.CreateClient();
        var path = "https://localhost:7072/forms/" + form_id;
        var response = await client.GetAsync(path, cancellation);
        
        var json = await response.Content.ReadAsStringAsync(cancellation);

        var cardJson = JsonConvert.DeserializeObject<FormResultDTO>(json).Result;

        var parsedCard = AdaptiveCard.FromJson(cardJson);

        foreach (var warning in parsedCard.Warnings)
        {
            this.logger.LogWarning($"Warning {(AdaptiveWarning.WarningStatusCode) warning.Code}" +
                $"received when parsing Adaptive Card {warning.Message}");
        }

        var attachment = MessageFactory.Attachment(new Attachment
        {
            ContentType = AdaptiveCard.ContentType,
            Content = parsedCard.Card,
        });

        await turnContext.SendActivityAsync(attachment, cancellation);
    }
}
