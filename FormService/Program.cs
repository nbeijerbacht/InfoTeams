using FormService.Logic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IAdaptiveCardRenderer, AdaptiveCardRenderer>();
builder.Services.AddSingleton<IElementRenderer, HeaderRenderer>();
builder.Services.AddSingleton<IElementRenderer, TextFieldRenderer>();
builder.Services.AddSingleton<IElementRenderer, IgnoreRenderer>();

builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
