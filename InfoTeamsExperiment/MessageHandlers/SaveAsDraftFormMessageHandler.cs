using Microsoft.Bot.Builder;
using Newtonsoft.Json;
using ZenyaBot.DTO;
using ZenyaBot.Interfaces;

namespace ZenyaBot.MessageHandlers;

/// <inheritdoc />
public class SaveAsDraftFormMessageHandler : ITeamsActionHandler
{
    private readonly IConfiguration config;
    private readonly ILogger<SaveAsDraftFormMessageHandler> logger;
    private readonly IHttpClientFactory clientFactory;

    public SaveAsDraftFormMessageHandler(
        IConfiguration config,
        ILogger<SaveAsDraftFormMessageHandler> logger,
        IHttpClientFactory clientFactory)
    {
        this.config = config;
        this.logger = logger;
        this.clientFactory = clientFactory;
    }

    /// <inheritdoc />
    public bool CanHandle(CustomActionType type) => type == CustomActionType.SaveAsDraft;

    private string FormServiceUrl => this.config.GetSection("Services")["FormService"];

    /// <inheritdoc />
    public async Task Handle(ITurnContext turnContext, IDictionary<string, object> messageData, CancellationToken cancellation = default)
    {
        var form_id = messageData["form_id"].ToString();

        var form = new SubmitFormDTO
        {
            form_id = form_id,
            is_draft = true,
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
        var path = this.FormServiceUrl + "form/";
        var response = await client.PostAsJsonAsync(path, form, cancellation);
        var json = await response.Content.ReadAsStringAsync();
        var test = JsonConvert.DeserializeObject<DraftDTO>(json);

        if (response.IsSuccessStatusCode)
        {
            await turnContext.SendActivityAsync($"https://msteams.zenya.work/Cases/FillOut/#/draft/?caseId={test.created_identifier}&caseSecret={test.case_secret}");
        }
        else
        {
            this.logger.LogError(response.ToString());
            await turnContext.SendActivityAsync("Something went wrong, please try again.");
        }

    }
}
