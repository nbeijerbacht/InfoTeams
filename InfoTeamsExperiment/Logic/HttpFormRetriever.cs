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
    private readonly ILogger<HttpFormRetriever> logger;
    private readonly IHttpClientFactory clientFactory;

    public HttpFormRetriever(ILogger<HttpFormRetriever> logger, IHttpClientFactory clientFactory)
    {
        this.logger = logger;
        this.clientFactory = clientFactory;
    }

    public async Task<AdaptiveCard> GetCardByFormId(string formId, string lookUpField = "", string lookUpQuery = "")
    {
        var client = this.clientFactory.CreateClient();
        var query = new QueryBuilder()
        {
            {"lookupFieldId", lookUpField },
            {"lookupFieldQuery", lookUpQuery },
        };
        var path = $"https://localhost:7072/form/{formId}?{query}";

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
        
        return parsedCard.Card;
    }
}
