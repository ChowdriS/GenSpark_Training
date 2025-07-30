using System;

namespace shop_api.Models;

public class CaptchaResponse
{
    public bool Success { get; set; }
    public List<string>? ErrorCodes { get; set; }
}
