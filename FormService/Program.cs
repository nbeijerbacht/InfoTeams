using FormService.Logic;
using FormService.Logic.ElementRenderers;
using FormService.Logic.FieldSerializers;
using FormService.Logic.ElementRenderers.Fields;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddSingleton<IAdaptiveCardRenderer, AdaptiveCardRenderer>();
        builder.Services.AddSingleton<ILookUpFieldInjector, LookupFieldInjector>();

        builder.Services.AddSingleton<IElementRenderer, HeaderRenderer>();
        builder.Services.AddSingleton<IElementRenderer, TextFieldRenderer>();
        builder.Services.AddSingleton<IElementRenderer, IgnoreRenderer>();
        builder.Services.AddSingleton<IElementRenderer, DateFieldRenderer>();
        builder.Services.AddSingleton<IElementRenderer, NumericFieldRenderer>();
        builder.Services.AddSingleton<IElementRenderer, ListFieldRenderer>();
        builder.Services.AddSingleton<IElementRenderer, SubjectTreeFieldRenderer>();
        builder.Services.AddSingleton<IElementRenderer, CheckboxFieldRenderer>();
        builder.Services.AddSingleton<IElementRenderer, TimeFieldRenderer>();

        builder.Services.AddSingleton<IElementRenderer, PositionFieldRenderer>();
        builder.Services.AddSingleton<IElementRenderer, UserFieldRenderer>();
        builder.Services.AddSingleton<ILookupFieldChoiceSearch, UserFieldRenderer>();

        builder.Services.AddSingleton<IFieldHandler, BooleanHandler>();
        builder.Services.AddSingleton<IFieldHandler, TextHandler>();
        builder.Services.AddSingleton<IFieldHandler, NumericHandler>();
        builder.Services.AddSingleton<IFieldHandler, DateHandler>();
        builder.Services.AddSingleton<IFieldHandler, ListOfStringsHandler>();

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

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}