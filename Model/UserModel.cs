using System.Text.Json.Serialization;

namespace ControlWork_9.Model;

public class UserModel
{
    public int Id { get; set; }
    public string AccountNumber { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
    public decimal Balance { get; set; } = 10000;
    [
    JsonIgnore]
    public string? RefreshToken { get; set; }

    public bool IsAdmin { get; set; } 


}