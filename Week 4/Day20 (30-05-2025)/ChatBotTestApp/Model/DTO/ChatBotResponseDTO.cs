using System;

namespace ChatBotTestApp.Model.DTO;
public class ChatBotResponseDTO
{
    public string? Question { get; set; }
    public string? Matched_Question { get; set; }
    public string? Answer { get; set; }
    public double Confidence { get; set; }
}
