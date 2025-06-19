using FastkartAPI.Infrastructure.Password;
using Microsoft.AspNetCore.Mvc;
using WebWritterAI.Services;
using WebWritterAI.Services.Services;

namespace WebWritterAI.Controllers;

[Controller]
[Route("[controller]")]
public class User : Controller
{
    private readonly UserService _userService;
    private readonly ILogger<AuthController> _logger;
    private readonly PasswordHasher _passwordHasher;
    private readonly PricingService _pricingService;

    public User(UserService userService, ILogger<AuthController> logger, PasswordHasher passwordHasher, PricingService pricingService)
    {
        _logger = logger;
        _userService = userService;
        _passwordHasher = passwordHasher;
        _pricingService = pricingService;
    }
    
    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var redirectResult = await RedirectIfNotLogged();
        if(redirectResult is not OkResult) 
            return redirectResult;
        
        return View("user");
    }
    
    [HttpGet("edit")]
    public async Task<IActionResult> Edit() 
    {
        var redirectResult = await RedirectIfNotLogged();
        if(redirectResult is not OkResult) 
            return redirectResult;
        
        return View("edit");
    }

    [HttpPost("edit")]
    public async Task<IActionResult> EditUser([FromForm] string name, [FromForm] string password)
    {
        _logger.LogInformation($"FullName: {name} Password: {password}");
        var id = _userService.GetUserByToken(User).Result;
        if(id == null) return Unauthorized();
        var idVal = id.Value;
        var user = _userService.GetById(idVal);
        
        if(password == null && name == null && (name.Length < 2 || password.Length < 8))
            return BadRequest("No info provided");
        
        if(name is { Length: >= 2 })
            user.Result.FullName = name;
        
        if(password is { Length: >= 8 })
        {
            var newPassword = _passwordHasher.GenerateTokenSHA(password);
            user.Result.Password = newPassword;
        }

        await _userService.Update(user.Result);
            
        return Redirect("user/profile");
    }

    [HttpPost("set-subscription")]
    public async Task<IActionResult> SetSubscription(Guid id)
    {
        var userId = _userService.GetUserByToken(User).Result;
        if(userId == null) return BadRequest();
        
        var pricing = _pricingService.GetPricing(id);
        if(pricing.Result == null) 
            return BadRequest("No such subscription");
        await _userService.SetSubscription(userId.Value, pricing.Result.Id);
        return Redirect("user/profile");
    }

    [HttpPost("cancel-subscription")]
    public async Task<IActionResult> CancelSubscription()
    {
        var id = _userService.GetUserByToken(User).Result;
        if(id == null) return BadRequest();
        var idVal = id.Value;
        await _userService.CancelSubscription(idVal);
        return Redirect("user/profile");
    }
    
    public Task<IActionResult> RedirectIfNotLogged()
    {
        var userId = _userService.GetUserByToken(User).Result;
        if (userId == null) return Task.FromResult<IActionResult>(Redirect("/auth/login"));
        return Task.FromResult<IActionResult>(Ok());
    }
}