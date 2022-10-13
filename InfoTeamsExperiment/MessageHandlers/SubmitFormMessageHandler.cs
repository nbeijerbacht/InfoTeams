using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ZenyaBot.DTO;
using ZenyaBot.Interfaces;

namespace ZenyaBot.MessageHandlers;

public class SubmitFormMessageHandler : ITeamsMessageHandler
{
    private readonly ILogger<SubmitFormMessageHandler> logger;
    private readonly IHttpClientFactory clientFactory;

    public SubmitFormMessageHandler(ILogger<SubmitFormMessageHandler> logger, IHttpClientFactory clientFactory)
    {
        this.logger = logger;
        this.clientFactory = clientFactory;
    }

    public bool CanHandle(MessageType type) => type == MessageType.SubmitForm;

    public async Task Handle(ITurnContext turnContext, IDictionary<string, object> messageData, CancellationToken cancellation = default)
    {
        var form_id = messageData["form_id"].ToString();

        var form = new SubmitFormDTO
        {
            form_id = form_id,
            fields = new List<FieldDTO>(),
        };

        foreach (var (key, value) in messageData)
        {
            if (int.TryParse(key, out int _) is true)
            {
                form.fields.Add(new FieldDTO
                {
                    field_id = key,
                    value = value,
                });
            }
        }

        var client = this.clientFactory.CreateClient();
        var path = "https://localhost:7072/form/";
        var response = await client.PostAsJsonAsync(path, form, cancellation);
        
        if (response.IsSuccessStatusCode)
        {
            await turnContext.SendActivityAsync("Form submitted succesfully");
        }
        else
        {
            this.logger.LogError(response.ToString());
            await turnContext.SendActivityAsync("Something went wrong, please try again.");
        }

    }
}
