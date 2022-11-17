using ZenyaBot;
using ZenyaBot.Commands;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.TeamsFx.Conversation;
using Microsoft.Bot.Builder;
using ZenyaBot.Interfaces;
using ZenyaBot.MessageHandlers;
using ZenyaBot.Logic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient("WebClient", client => client.Timeout = TimeSpan.FromSeconds(600));
builder.Services.AddHttpContextAccessor();

// Prepare Configuration for ConfigurationBotFrameworkAuthentication
builder.Configuration["MicrosoftAppType"] = "MultiTenant";
builder.Configuration["MicrosoftAppId"] = builder.Configuration.GetSection("BOT_ID")?.Value;
builder.Configuration["MicrosoftAppPassword"] = builder.Configuration.GetSection("BOT_PASSWORD")?.Value;

// Create the Bot Framework Authentication to be used with the Bot Adapter.
builder.Services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();

// Create the Cloud Adapter with error handling enabled.
// Note: some classes expect a BotAdapter and some expect a BotFrameworkHttpAdapter, so
// register the same adapter instance for both types.
builder.Services.AddSingleton<CloudAdapter, AdapterWithErrorHandler>();
builder.Services.AddSingleton<IBotFrameworkHttpAdapter>(sp => sp.GetService<CloudAdapter>()!);
//builder.Services.AddSingleton<BotAdapter>(sp => sp.GetService<CloudAdapter>());

// Create command handlers and the Conversation with command-response feature enabled.
builder.Services.AddSingleton<ITeamsCommandHandler, SearchFormCommandsHandler>();

// Create message handlers to handle actions from Adaptive Cards.
builder.Services.AddSingleton<ITeamsActionHandler, ShowFormActionHandler>();
builder.Services.AddSingleton<ITeamsActionHandler, SubmitFormActionHandler>();
builder.Services.AddSingleton<ITeamsActionHandler, LookUpFieldMessageHandler>();
builder.Services.AddSingleton<ITeamsActionHandler, SaveAsDraftFormMessageHandler>();

builder.Services.AddSingleton<IFormRetriever, HttpFormRetriever>();
builder.Services.AddSingleton<IFormFiller, FormFiller>();


// Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
builder.Services.AddTransient<IBot, TeamsBot>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();