using System.Threading.Tasks;
using ChatBotTestApp.Interface;
using ChatBotTestApp.Model.DTO;
using ChatBotTestApp.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotTestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;

        public ChatBotController(IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [HttpPost]
        public async Task<IActionResult> GetReply(ChatBotRequestDTO requestDTO)
        {
            try
            {
                var response = await _chatBotService.GetFaqReply(requestDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error contacting external API. {ex.Message}");
            }
        }
    }
}
