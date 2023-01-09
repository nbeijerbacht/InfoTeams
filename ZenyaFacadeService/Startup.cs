using Microsoft.Extensions.Configuration;
using ZenyaFacadeService.HttpClient;

namespace ZenyaFacadeService;

public class Startup
{
    public IConfiguration configRoot
    {
        get;
    }
    public Startup(IConfiguration configuration)
    {
        configRoot = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddScoped<IZenyaFormHttpClient, ZenyaFormHttpClient>();
        services.AddScoped<IZenyaLookupHttpClient, ZenyaLookupHttpClient>();
        services.AddControllers();
        services.AddHttpClient("ZenyaClient", c =>
        {
            c.DefaultRequestHeaders.Add("X-Api-Version", "3");
            c.DefaultRequestHeaders.Add("Authorization", configRoot.GetValue<string>("ZENYA_API_TOKEN"));
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
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
