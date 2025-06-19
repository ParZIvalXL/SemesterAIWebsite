using Microsoft.AspNetCore.Mvc;
using WebWritterAI.Services;
using WebWritterAI.Services.Services;

namespace WebWritterAI.Controllers;

[Controller]
[Route("[controller]")]
public class AdminController : Controller
{
    UserService _userService;
    PricingService _pricingService;
    private ILogger<AdminController> _logger;

    public AdminController(UserService userService, ILogger<AdminController> logger, PricingService pricingService)
    {
        _userService = userService;
        _logger = logger;
        _pricingService = pricingService;
    }
    
    [HttpGet("panel")]
    public IActionResult Panel() 
    {
        var user = _userService.GetUserByToken(User).Result;
        if (user == null) 
            return Unauthorized();
        var userData = _userService.GetById(user.Value).Result;
        if(userData.Role != "admin") 
            return Unauthorized();

        return View();
    }

    [HttpGet("users")]
    public async Task<IActionResult> Users(int page, int perPage)
    {
        var user = _userService.GetUserByToken(User).Result;
        if (user == null) 
            return Unauthorized();
        var userData = _userService.GetById(user.Value).Result;
        if(userData.Role != "admin") 
            return Unauthorized();

        _logger.LogInformation($"Page: {page} PerPage: {perPage}");
        var data = await _userService.GetAll();
        data = data.Skip((page) * perPage).Take(perPage).ToList();
        return Json(data);
    }
    [HttpGet("users/count")]
    public async Task<IActionResult> UsersCount()
    {
        var user = _userService.GetUserByToken(User).Result;
        if (user == null) 
            return Unauthorized();
        var userData = _userService.GetById(user.Value).Result;
        if(userData.Role != "admin") 
            return Unauthorized();

        var data = await _userService.GetCount();
        return Json(data);
    }
    
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = _userService.GetUserByToken(User).Result;
        if (user == null) 
            return Unauthorized();
        var userData = _userService.GetById(user.Value).Result;
        if(userData.Role != "admin") 
            return Unauthorized();
        
        _logger.LogInformation($"Id: {id}");
        await _userService.Delete(id);
        return Ok("User deleted");
    }
}