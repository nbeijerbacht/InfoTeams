using System.Text.Json;

namespace ZenyaFacadeService.HttpClient;

public interface IZenyaLookupHttpClient
{
    Task<string> FindUser(string? search);
    Task<string> FindExternalSource(string externalSourceId);
    Task<string> FindTeam(string? search);

    Task<string> GetPositions();

}