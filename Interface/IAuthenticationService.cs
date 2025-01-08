using ControlWork_9.Model;
using ControlWork_9.Response;
using ControlWork_9.ViewModel;

namespace WebApi.Interfaces;

public interface IAuthenticationService
{
    Task<BaseApiResponse<string>> CreateUserAsync(UserRegisterData urgd);
    LoginResponse ValidateToken(string token);
    Task<LoginResponse> LoginAsync(LoginModelData loginModelData);
    UserModel GetUserModel(int id);
    List<UserModel> GetAllUsers();
}