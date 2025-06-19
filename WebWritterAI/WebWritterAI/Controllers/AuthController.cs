using DataBase.Models;
using FastkartAPI.Infrastructure.Password;
using WebWritterAI.Services.Services;
using Microsoft.AspNetCore.Mvc;
using WebWritterAI.Contracts.Contracts;

namespace WebWritterAI.Controllers;

[Controller]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly UserService _userService;
    private readonly ILogger<AuthController> _logger;
    private readonly PasswordHasher _passwordHasher;

    public AuthController(UserService userService, ILogger<AuthController> logger, PasswordHasher passwordHasher)
    {
        _logger = logger;
        _userService = userService;
        _passwordHasher = passwordHasher;
    }
    
    [HttpGet("login")]
    public async Task<IActionResult> GetLogin()
    {
        /*if(Response.Cookies != null)
        {
            var userIdFromDb = _userService.GetUserByToken(User).Result.Value;
            if (!string.IsNullOrEmpty(userIdFromDb.ToString()))
                return RedirectToAction("/");
        }*/
        
        return View("login");
    }
    
    [HttpGet("register")]
    public async Task<IActionResult> GetRegister() => View("register");
    
    [HttpGet("reset")]
    public async Task<IActionResult> GetReset() => View("reset");

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] string Email, [FromForm] string Password)
    {
        var User = new LoginContract() { Email = Email, Password = Password };
        _logger.LogInformation($"Email: {User.Email} Password: {User.Password}");

        try
        {
            var res = await _userService.Login(User);

            Response.Cookies.Append("ass-token", res);
        
            return Redirect("/");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Произошла ошибка при обработке запроса");
            return BadRequest(e.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] string Fullname, [FromForm] string Email, [FromForm] string Password)
    {
        var user = new RegitsterContract() { FullName = Fullname, Email = Email, Password = Password };
        _logger.LogInformation($"Email: {user.Email} Password: {user.Password}");
        var email = _userService.GetByEmail(user.Email);
        if (email.Result != null)
        {
            return BadRequest("Пользователь с таким email уже зарегистрирован");
        }
        
        await _userService.Register(user);
        
        return Redirect("/success");
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset([FromForm] string email, [FromForm] string password)
    {
        var token = _userService.GetByEmail(email);
        if (token.Result == null)
        {
            return BadRequest("Такого пользователя с почтой не существует");
        }
        UserModel user = token.Result;
        
        var newPassword = _passwordHasher.GenerateTokenSHA(password);
        user.Password = newPassword;
        await _userService.Update(user);
        
        return Ok($"Пароль был изменен");
    }
    
    [HttpGet("user")]
    public async Task<IActionResult> GetUserById()
    {
        var id = _userService.GetUserByToken(User).Result;
        if(id == null) return BadRequest();
        var idVal = id.Value;
        return Ok(await _userService.GetById(idVal));
    }
    
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("ass-token");
        return Redirect("/");
    }
}