using System.ComponentModel.DataAnnotations;

namespace ControlWork_9.ViewModel;

public class UserRegisterData
{
    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; init; }
    
    [Compare(nameof(Password), ErrorMessage = "Password should be the same")]
    public required string ConfirmPassword { get; init; }


}