using ControlWork_9.Infrastructure;
using ControlWork_9.Model;
using ControlWork_9.Response;
using ControlWork_9.ViewModel;
using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;

namespace ControlWork_9.Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly PaymentDBContext _dbContext;
    private readonly IJwtProvider _jwtProvider;
    private readonly PasswordHasher _passwordHasher;

    public AuthenticationService(PaymentDBContext dbContext, IJwtProvider jwtProvider, PasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _dbContext = dbContext;
    }

    public List<UserModel> GetAllUsers()
    {
        return _dbContext.Users.ToList();
        
    }
    public UserModel GetUserModel(int id)
    {
        return _dbContext.Users.FirstOrDefault(x => x.Id == id);
    }

    public async Task<BaseApiResponse<string>> CreateUserAsync(UserRegisterData urgd)
    {
        Random random = new Random();

        string chars = "0123456789";
        var accNumber = new string(Enumerable.Repeat(chars, 13)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        UserModel user = new UserModel()
        {
            AccountNumber = accNumber,
            Password = urgd.Password
        };
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();


        // if (identity is not null)
        //     return new BaseApiResponse<Guid>(Guid.Empty, "error: User with this email already exists");
        //
        //
        return new BaseApiResponse<string>(user.AccountNumber, "success");
    }

    public LoginResponse ValidateToken(string token)
    {
        var response = _jwtProvider.ValidateToken(token);
        var loginResponse = new LoginResponse
        {
            IsLoggedIn = response
        };
        return loginResponse;
    }

    public async Task<LoginResponse> LoginAsync(LoginModelData loginModelData)
    {
        var user = _dbContext.Users
            .FirstOrDefault(x => x.AccountNumber == loginModelData.AccountNumber);

        if (user is null)
            return new LoginResponse(false, errorMessage: "error: User with this account number not found");
        return await LoginUserAsync(user, loginModelData.Password);
    }
    

    private async Task<LoginResponse> LoginUserAsync(UserModel user, string inputPassword)
    {
        //var response = _passwordHasher.VerifyHash(inputPassword, user.Password);
        //if (!response)
        if (user.Password != inputPassword)
            return new LoginResponse(false, errorMessage: "Wrong Password");

        var token = _jwtProvider.GenerateUserToken(user);
        var refreshToken = _jwtProvider.GenerateRefreshToken();

        // dbUser.LastLogin = new DateOnly(_now.Year, _now.Month, _now.Day);
        user.RefreshToken = refreshToken;

       
        _dbContext.Entry(user).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        //await _userRepository.UpdateUserAsync(user);
        return new LoginResponse(true, jwtToken: token, refreshToken: refreshToken);
    }
    
}