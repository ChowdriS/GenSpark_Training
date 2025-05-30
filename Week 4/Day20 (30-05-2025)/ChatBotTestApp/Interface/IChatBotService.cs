using System;
using ChatBotTestApp.Model.DTO;

namespace ChatBotTestApp.Interface;

public interface IChatBotService
{
    public Task<string> GetFaqReply(ChatBotRequestDTO request);
}
