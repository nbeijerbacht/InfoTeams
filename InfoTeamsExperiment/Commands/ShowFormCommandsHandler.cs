using ZenyaBot.Models;
using AdaptiveCards.Templating;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.TeamsFx.Conversation;
using Newtonsoft.Json;
using AdaptiveCards;

namespace ZenyaBot.Commands
{
    /// <summary>
    /// The <see cref="ShowFormCommandsHandler"/> registers a pattern with the <see cref="ITeamsCommandHandler"/> and 
    /// responds with an Adaptive Card if the user types the <see cref="TriggerPatterns"/>.
    /// </summary>
    public class ShowFormCommandsHandler : ITeamsCommandHandler
    {
        private readonly ILogger<ShowFormCommandsHandler> _logger;
        private readonly string _adaptiveCardFilePath = Path.Combine(".", "Resources", "HelloWorldCard.json");

        private const string commandName = "form";

        public IEnumerable<ITriggerPattern> TriggerPatterns => new List<ITriggerPattern>
        {
            // Used to trigger the command handler if the command text contains 'helloWorld'
            new RegExpTrigger("^" + commandName)
        };

        public ShowFormCommandsHandler(ILogger<ShowFormCommandsHandler> logger)
        {
            _logger = logger;
        }

        public async Task<ICommandResponse> HandleCommandAsync(ITurnContext turnContext, CommandMessage message, CancellationToken cancellationToken = default)
        {
            HttpClient client = new HttpClient();
            var path = "https://localhost:7242/";
            var response = await client.GetAsync(path);

            var searchString = message.Text.Substring(commandName.Length).Trim();

            _logger?.LogInformation($"Bot received search: {searchString}");


            // Read adaptive card template
            var cardTemplate = await File.ReadAllTextAsync(_adaptiveCardFilePath, cancellationToken);

            // Render adaptive card content
            var cardContent = new AdaptiveCardTemplate(cardTemplate).Expand
            (
                new HelloWorldModel
                {
                    Title = $"You searched for \"{searchString}\".",
                    Body = "",
                }
            );

            // Build attachment
            var activity = MessageFactory.Attachment
            (
                new Attachment
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = await response.Content.ReadAsStringAsync(),
                }
            );

            // send response
            return new ActivityCommandResponse(activity);
        }
    }
}
