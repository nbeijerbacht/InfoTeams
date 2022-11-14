using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ZenyaFacadeService.DTO;
using ZenyaFacadeService.HttpClient;

[ApiController]
[Route("[controller]")]
public class LookupInformationController : ControllerBase
{
    private readonly ILogger<LookupInformationController> _logger;
    private readonly IZenyaLookupHttpClient _client;

    public LookupInformationController(ILogger<LookupInformationController> logger, IZenyaLookupHttpClient client)
    {
        _logger = logger;
        _client = client;
    }

    [HttpGet]
    public async Task<IEnumerable<UserDTO>> SearchThroughForms([FromQuery(Name = "search")] string search)
    {
        var json = await _client.FindUser(search);
        var formData = JsonConvert.DeserializeObject<List<UserDTO>>(json);

        return formData;
    }
}