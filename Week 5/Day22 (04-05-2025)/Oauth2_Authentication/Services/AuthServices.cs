using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Oauth2_Authentication.Misc;
using System.Security.Claims;
using Oauth2_Authentication.Interface;

public class AuthService : IAuthService
{
    private readonly TokenGenerator _tokenGenerator;

    public AuthService(TokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
    }

    public async Task<string> HandleGoogleLoginAsync(HttpContext httpContext)
    {
        var result = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.Succeeded)
        {
            throw new Exception("Authentication failed.");
        }

        var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            throw new Exception("Email claim not found.");
        }

        return _tokenGenerator.GenerateJwtToken(email);
    }
}
