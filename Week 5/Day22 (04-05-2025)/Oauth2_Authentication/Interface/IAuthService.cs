using System;

namespace Oauth2_Authentication.Interface;

public interface IAuthService
{
    public Task<string> HandleGoogleLoginAsync(HttpContext httpContext);
}