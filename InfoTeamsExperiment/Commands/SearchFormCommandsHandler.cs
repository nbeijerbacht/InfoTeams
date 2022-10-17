using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.TeamsFx.Conversation;
using Newtonsoft.Json;
using AdaptiveCards;
using ZenyaBot.DTO;
using ZenyaBot.Interfaces;

namespace ZenyaBot.Commands;

/// <summary>
/// The <see cref="SearchFormCommandsHandler"/> registers a pattern with the <see cref="ITeamsCommandHandler"/> and 
/// responds with an Adaptive Card if the user types the <see cref="TriggerPatterns"/>.
/// </summary>
public class SearchFormCommandsHandler : ITeamsCommandHandler
{
    private readonly ILogger<SearchFormCommandsHandler> _logger;
    private readonly IHttpClientFactory clientFactory;
    private const string commandName = "search";

    public IEnumerable<ITriggerPattern> TriggerPatterns => new List<ITriggerPattern>
    {
        // Used to trigger the command handler if the command text contains 'helloWorld'
        new RegExpTrigger("^" + commandName)
    };

    public SearchFormCommandsHandler(ILogger<SearchFormCommandsHandler> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        this.clientFactory = clientFactory;
    }

    public async Task<ICommandResponse> HandleCommandAsync(ITurnContext turnContext, CommandMessage message, CancellationToken cancellationToken = default)
    {
        var searchString = message.Text.Substring(commandName.Length).Trim();
        _logger?.LogInformation($"Bot received search: {searchString}");

        var client = this.clientFactory.CreateClient();
        var path = "https://localhost:7072/form?search=" + searchString;
        var response = await client.GetAsync(path);


        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var adaptiveCards = JsonConvert.DeserializeObject<SearchResultDTO>(json);

        if (adaptiveCards.Results.Count == 0)
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 1))
            {
                Body =
                {
                    new AdaptiveTextBlock("Nothing found for your search")
                    {
                        Size = AdaptiveTextSize.Medium,
                        Color = AdaptiveTextColor.Warning,
                    }
                }
            };
            return new ActivityCommandResponse(
                MessageFactory.Attachment(new Attachment()
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card,
                })
            );
        }

        var attachments = adaptiveCards.Results
            .Select(adaptiveCardJson => AdaptiveCard.FromJson(adaptiveCardJson).Card)
            .Select(card => {
                card.Actions.Add(new AdaptiveSubmitAction
                {
                    Title = "Fill in",
                    DataJson = JsonConvert.SerializeObject(new {
                        type = CustomActionType.SelectForm.ToString(),
                        form_id = card.Id,
                    }),
                });
                return card;
            }).Select(card => new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card,
            }).ToList();

        // Build attachment
        var activity = MessageFactory.Attachment(attachments);
        return new ActivityCommandResponse(activity);
    }
}
