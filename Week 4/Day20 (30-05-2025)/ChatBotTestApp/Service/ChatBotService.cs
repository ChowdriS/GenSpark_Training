using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ChatBotTestApp.Model.DTO;
using ChatBotTestApp.Interface;

namespace ChatBotTestApp.Service;

public class ChatBotService : IChatBotService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ChatBotService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetFaqReply(ChatBotRequestDTO request)
    {
        var client = _httpClientFactory.CreateClient();
        var json = JsonConvert.SerializeObject(new { question = request.Question });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("http://127.0.0.1:8000/query", content);
        return await response.Content.ReadAsStringAsync();
    }
}
