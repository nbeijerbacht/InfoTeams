using FormService.Logic;
using FormService.Logic.Field_Handlers;
using FormService.Logic.Field_Type;
using FormService.Logic.Field_Types;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IAdaptiveCardRenderer, AdaptiveCardRenderer>();

builder.Services.AddSingleton<IElementRenderer, HeaderRenderer>();
builder.Services.AddSingleton<IElementRenderer, TextFieldRenderer>();
builder.Services.AddSingleton<IElementRenderer, IgnoreRenderer>();
builder.Services.AddSingleton<IElementRenderer, DateFieldRenderer>();
builder.Services.AddSingleton<IElementRenderer, NumericFieldRenderer>();
builder.Services.AddSingleton<IElementRenderer, ListFieldRenderer>();
builder.Services.AddSingleton<IElementRenderer, SubjectTreeFieldRenderer>();
builder.Services.AddSingleton<IElementRenderer, CheckboxFieldRenderer>();
builder.Services.AddSingleton<IElementRenderer, TimeFieldRenderer>();

builder.Services.AddSingleton<IFieldHandler, TextHandler>();
builder.Services.AddSingleton<IFieldHandler, NumericHandler>();
builder.Services.AddSingleton<IFieldHandler, DateHandler>();

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
