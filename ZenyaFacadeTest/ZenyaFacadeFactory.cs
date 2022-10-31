using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using ZenyaFacadeService.HttpClient;

namespace ZenyaFacadeTest
{
    internal class ZenyaFacadeFactory : WebApplicationFactory<ZenyaFacadeService.Startup>
    {
        public IConfiguration Configuration { get; private set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var clientMock = new Mock<IZenyaHttpClient>();
                clientMock.Setup(c => c.GetAllForms()).ReturnsAsync(MockData.mockAllForms);
                clientMock.Setup(c => c.GetFormById(2216)).ReturnsAsync(MockData.mockFormById);
                clientMock.Setup(c => c.PostForm(It.IsAny<JsonElement>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
                services.AddTransient(s => clientMock.Object);
            });
        }
    }
}
