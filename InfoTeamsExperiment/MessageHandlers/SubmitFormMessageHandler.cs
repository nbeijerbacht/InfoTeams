using Microsoft.Bot.Builder;
using ZenyaBot.DTO;
using ZenyaBot.Interfaces;

namespace ZenyaBot.MessageHandlers;

/// <inheritdoc />
public class SubmitFormActionHandler : ITeamsActionHandler
{
    private readonly IConfiguration config;
    private readonly ILogger<SubmitFormActionHandler> logger;
    private readonly IHttpClientFactory clientFactory;

    public SubmitFormActionHandler(
        IConfiguration config,
        ILogger<SubmitFormActionHandler> logger,
        IHttpClientFactory clientFactory)
    {
        this.config = config;
        this.logger = logger;
        this.clientFactory = clientFactory;
    }

    /// <inheritdoc />
    public bool CanHandle(CustomActionType type) => type == CustomActionType.SubmitForm;

    private string FormServiceUrl => this.config.GetSection("Services")["FormService"];

    /// <inheritdoc />
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
            if (int.TryParse(key, out int _) is true && value is not "")
            {
                form.fields.Add(new FieldDTO
                {
                    field_id = key,
                    value = value,
                });
            }
        }

        var client = this.clientFactory.CreateClient();
        var path = this.FormServiceUrl + "form/";
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
