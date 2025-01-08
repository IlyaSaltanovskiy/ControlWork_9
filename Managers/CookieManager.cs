using ControlWork_9.Service;

namespace ControlWork_9.Managers;

public static class CookieManager
{
    public static string GetUserIdHash(this HttpContext context)
    {
        var provider = new JwtProvider();
        context.Request.Cookies.TryGetValue("YouAreGreat", out var accessToken);
        var principal = provider.GetPrincipalFromToken(accessToken!);
        var idHashString = principal?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        return idHashString;
    }
}