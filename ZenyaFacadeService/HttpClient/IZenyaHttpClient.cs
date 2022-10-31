using System.Text.Json;

namespace ZenyaFacadeService.HttpClient
{
    public interface IZenyaHttpClient
    {
        Task<string> GetAllForms();
        Task<string> GetFormById(int id);
        Task<HttpResponseMessage> PostForm(JsonElement body);
    }
}