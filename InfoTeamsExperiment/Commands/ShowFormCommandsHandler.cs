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

            var searchString = message.Text.Substring(commandName.Length).Trim();


            HttpClient client = new HttpClient();
            var path = "https://localhost:7242/";
            var response = await client.GetAsync(path);

            _logger?.LogInformation($"Bot received search: {searchString}");

            var json = await response.Content.ReadAsStringAsync();
            var form_data = JsonConvert.DeserializeObject<object>(json);

            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));

            // Build attachment
            var activity = MessageFactory.Attachment
            (
                new Attachment
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card,
                }
            );

            return new ActivityCommandResponse(activity);
        }
    }
}
