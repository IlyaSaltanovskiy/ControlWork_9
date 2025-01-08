using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ControlWork_9.Options;

public class AuthOptions
{
    private const string Key = "KHa7O67Lwgl1XTbwDhLWhq8vKl9MWgeG08aHqwQOhvQ";
    
    public const string Issuer = "ProjectSuccess";
    public const string Audience = "SuccessClient";
    public const int Lifetime = 15;
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }

}