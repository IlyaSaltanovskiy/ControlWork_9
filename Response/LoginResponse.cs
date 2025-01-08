namespace ControlWork_9.Response;

public class LoginResponse
{
    
    public bool IsLoggedIn { get; set; }
    public string? ErrorMessage { get; set; }
    public string? JwtToken { get; set; }
    public string? RefreshToken { get; set; }
    public string? Role { get; set; }

    public LoginResponse(
        bool isLoggedIn = false, 
        string? errorMessage = null, 
        string? jwtToken = null, 
        string? refreshToken = null,
        string? role = null
    )
    {
        IsLoggedIn = isLoggedIn;
        ErrorMessage = errorMessage;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
        Role = role;
    }

}