using System.Text.Json;

namespace ZenyaFacadeService.HttpClient;

public interface IZenyaLookupHttpClient
{
    Task<string> FindUser(string search);
}