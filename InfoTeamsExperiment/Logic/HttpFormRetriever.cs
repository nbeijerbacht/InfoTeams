using AdaptiveCards;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Bot.Builder;
using Newtonsoft.Json;
using System.Net;
using ZenyaBot.DTO;
using ZenyaBot.Exceptions;
using ZenyaBot.Interfaces;
using ZenyaBot.MessageHandlers;

namespace ZenyaBot.Logic;

public class HttpFormRetriever : IFormRetriever
{
    private readonly IConfiguration config;
    private readonly ILogger<HttpFormRetriever> logger;
    private readonly IHttpClientFactory clientFactory;

    public HttpFormRetriever(
        IConfiguration config,
        ILogger<HttpFormRetriever> logger,
        IHttpClientFactory clientFactory)
    {
        this.config = config;
        this.logger = logger;
        this.clientFactory = clientFactory;
    }

    private string FormServiceUrl => this.config.GetSection("Services")["FormService"];

    public async Task<AdaptiveCard> GetCardByFormId(string formId, string lookUpField = "", string lookUpQuery = "")
    {
        var client = this.clientFactory.CreateClient();
        var query = new QueryBuilder()
        {
            {"lookupFieldId", lookUpField },
            {"lookupFieldQuery", lookUpQuery },
        };
        var path = $"{this.FormServiceUrl}form/{formId}{query}";

        var response = await client.GetAsync(path);

        var json = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new FormNotFound(formId);

        var result = JsonConvert.DeserializeObject<FormResultDTO>(json).Result;

        var parsedCard = AdaptiveCard.FromJson(result);

        foreach (var warning in parsedCard.Warnings)
        {
            this.logger.LogWarning($"Warning {(AdaptiveWarning.WarningStatusCode)warning.Code}" +
                $"received when parsing Adaptive Card {warning.Message}");
        }

        parsedCard.Card.Actions.Add(new AdaptiveSubmitAction
        {
            Title = "Submit",
            Id = formId,
            DataJson = JsonConvert.SerializeObject(new
            {
                type = CustomActionType.SubmitForm.ToString(),
                formId,
            }),
            Style = "positive",
        });

        parsedCard.Card.Actions.Add(new AdaptiveSubmitAction
        {
            Title = "Save as Draft",
            Id = formId + "-draft",
            DataJson = JsonConvert.SerializeObject(new
            {
                type = CustomActionType.SaveAsDraft.ToString(),
                formId,
            }),
        });

        return parsedCard.Card;
    }
}
