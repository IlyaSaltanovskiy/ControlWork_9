using System.ComponentModel.DataAnnotations;

namespace ControlWork_9.ViewModel;

public class LoginModelData
{
    [Required(ErrorMessage = "AccountNumber is required")]
    public required string AccountNumber { get; init; }
    
    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; init; }

}