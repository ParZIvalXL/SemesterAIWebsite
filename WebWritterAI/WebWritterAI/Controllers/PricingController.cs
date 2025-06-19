using DataBase.Repository;
using Microsoft.AspNetCore.Mvc;
using WebWritterAI.Services;
using WebWritterAI.Services.Services;

namespace WebWritterAI.Controllers.Services;

[Controller]
[Route("[controller]")]
public class PricingController : Controller
{
    private readonly PricingService _pricingService;
    private readonly UserService _userService;
    private readonly ILogger<PricingController> _logger;
    
    public PricingController(PricingService pricingService, UserService userService, ILogger<PricingController> logger)
    {
        _pricingService = pricingService;
        _userService = userService;
        _logger = logger;
    }
    
    [HttpGet("subscriptions")]
    public async Task<IActionResult> Subscriptions()
    {
        var redirectResult = await RedirectIfNotLogged();
        if(redirectResult is not OkResult) 
            return redirectResult;
        
        var tiers = await _pricingService.GetPricings();
        return View("Subscriptions", tiers);
    }
    
    [HttpGet("subscription")]
    public async Task<IActionResult> Subscription(Guid id)
    {
        var tier = await _pricingService.GetPricing(id);
        return Json(tier);
    }
    
    [HttpPost("buy-subscription")]
    public async Task<IActionResult> BuySubscription(Guid id)
    {
        _logger.LogInformation($"Id: {id}");
        var userId = _userService.GetUserByToken(User).Result;
        if (userId == null) return BadRequest();
        var user = _userService.GetById(userId.Value);
        if(user.Result.Role == id.ToString()) return BadRequest("You already have this subscription");
        var tier = await _pricingService.GetPricing(id);
        if (tier == null) return BadRequest("No such subscription");
        await _userService.SetSubscription(userId.Value, tier.Id);
        return Ok("You have successfully subscribed");
    }

    public Task<IActionResult> RedirectIfNotLogged()
    {
        var userId = _userService.GetUserByToken(User).Result;
        if (userId == null) return Task.FromResult<IActionResult>(Redirect("/auth/login"));
        return Task.FromResult<IActionResult>(Ok());
    }
}