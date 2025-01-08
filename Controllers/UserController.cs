using ControlWork_9.Managers;
using ControlWork_9.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebApi.Interfaces;

namespace ControlWork_9.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public UserController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [ActionName("RegisterUser")]
    public async Task<IActionResult> CreateNewUser([FromBody] UserRegisterData urgd)
    {
        try
        {
            var result = await _authenticationService.CreateUserAsync(urgd);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModelData modelData)
    {
        try
        {
            HttpContext.Request.Cookies.TryGetValue("YouAreAwesome", out var refreshJwtToken);
            HttpContext.Request.Cookies.TryGetValue("YouAreGreat", out var accessToken);
            if (refreshJwtToken is not null && accessToken is not null)
            {
                var loginResponse = _authenticationService.ValidateToken(accessToken);

                if (loginResponse.IsLoggedIn)
                {
                    loginResponse.RefreshToken = refreshJwtToken;
                    loginResponse.JwtToken = accessToken;
                    loginResponse.ErrorMessage = "Already logging in";
                    return Ok(loginResponse);
                }
            }

            var response = await _authenticationService.LoginAsync(modelData);
            if (response.IsLoggedIn is false)
                return BadRequest(response.ErrorMessage);

            var token = response.JwtToken;
            var refreshToken = response.RefreshToken;

            Response.Cookies.Append("YouAreGreat", token!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                Path = "/",
                Domain = "localhost"
            });

            Response.Cookies.Append("YouAreAwesome", refreshToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                Path = "/",
                Domain = "localhost"
            });
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [ActionName("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("YouAreGreat");
        HttpContext.Response.Cookies.Delete("YouAreAwesome");
        return Ok();
    }

    [HttpGet]
    [ActionName("Info")]
    public async Task<IActionResult> Info()
    {
        var idHash = HttpContext.GetUserIdHash();

        return Ok(idHash);
    }

    [HttpGet]
    [ActionName("GetCurrentUser")]
    public IActionResult GetCurrentUser()
    {
        var idHash = HttpContext.GetUserIdHash();
        var user = _authenticationService.GetUserModel(Convert.ToInt32(idHash));
        return new JsonResult(new { AccountNumber = user.AccountNumber, Balance = user.Balance });
    }
    [HttpGet]
    [ActionName("GetAllUsers")]
    public IActionResult GetAllUsers()
    {
        var idHash = HttpContext.GetUserIdHash();
        var list = _authenticationService.GetAllUsers();
        return Ok(list);
    }

}