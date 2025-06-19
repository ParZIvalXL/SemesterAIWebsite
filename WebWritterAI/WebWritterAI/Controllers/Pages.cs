using Microsoft.AspNetCore.Mvc;
using WebWritterAI.Services;

namespace WebWritterAI.Controllers.Services;

[Controller]
[Route("/")]
public class Pages : Controller
{
    private readonly PricingService _pricingService;
    
    public Pages(PricingService pricingService)
    {
        _pricingService = pricingService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var pricings = await _pricingService.GetPricings();
        return View("index", pricings);
    }
    
    [HttpGet("about")]
    public IActionResult About()
    {
        return View();
    }
    
    [HttpGet("terms")]
    public IActionResult Terms()
    {
        return View("terms-condition");
    }
    
    [HttpGet("success")]
    public IActionResult Success()
    {
        return View();
    }
    
    [HttpGet("contact")]
    public IActionResult Contact()
    {
        return View();
    }
    
    [HttpGet("404")]
    public IActionResult NotFoundPage()
    {
        return View("404");
    }
    
    [HttpGet("504")]
    public IActionResult ServerError()
    {
        return View("504");
    }
    
    [HttpGet("features")]
    public IActionResult Features()
    {
        return View();
    }
    
    [HttpGet("usecase")]
    public IActionResult UseCase()
    {
        return View();
    }
    
    [HttpGet("usecase-details")]
    public IActionResult UseCaseDetails()
    {
        return View("usecase-details");
    }
}