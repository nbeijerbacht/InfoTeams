using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.TeamsFx.Conversation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZenyaBot.Interfaces;

namespace ZenyaBot;

/// <summary>
/// An empty bot handler.
/// You can add your customization code here to extend your bot logic if needed.
/// </summary>
public class TeamsBot : ActivityHandler
{
    private readonly List<ITeamsCommandHandler> commands;
    private readonly IEnumerable<ITeamsActionHandler> messageHandlers;
    private readonly ILogger<TeamsBot> logger;

    public TeamsBot(
        IEnumerable<ITeamsCommandHandler> commands,
        IEnumerable<ITeamsActionHandler> messageHandlers,
        ILogger<TeamsBot> logger)
    {
        this.commands = commands.ToList();
        this.messageHandlers = messageHandlers;
        this.logger = logger;
    }

    /// <summary>
    /// Handle when a message is addressed to the bot.
    /// </summary>
    /// <param name="turnContext">The turn context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the work queued to execute.</returns>
    protected override Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        if (turnContext.Activity.Text is string text)
            return ExecuteCommand();

        else if (turnContext.Activity.Value is object value)
            return HandleMessage(value);

        this.logger.LogWarning("Bot received a message without text and without value");
        return Task.CompletedTask;
        
        async Task ExecuteCommand()
        {
            ITeamsCommandHandler? handler = this.commands
                   .FirstOrDefault(handler => handler.TriggerPatterns.Any(p => p.ShouldTrigger(text)));

            if (handler is null)
                this.logger.LogWarning($"No handler found that can handle message with text='{text}'");
            else
            {
                var response = await handler.HandleCommandAsync(
                    turnContext,
                    new CommandMessage
                    {
                        Text = text,
                    },
                    cancellationToken);

                if (response is not null) await response.SendResponseAsync(turnContext, cancellationToken);
            }

        }

        async Task HandleMessage(object value)
        {
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(value.ToString()!);

            var messageType = Enum.Parse<CustomActionType>(json["type"].ToString());

            var handler = this.messageHandlers.FirstOrDefault(h => h.CanHandle(messageType));
            if (handler is null)
            {
                await turnContext.SendActivityAsync("Could not handle message: " + value.ToString(), cancellationToken: cancellationToken);
            }
            else
            {
                await handler.Handle(turnContext, json, cancellationToken);
            }
        }
    }
}
