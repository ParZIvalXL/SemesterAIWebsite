using Microsoft.AspNetCore.Mvc;

namespace WebWritterAI.Controllers.Services;

[Controller]
[Route("/")]
public class Pages : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
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
    
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpGet("register")]
    public IActionResult Register()
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
    
    [HttpGet("reset")]
    public IActionResult Reset()
    {
        return View();
    }
    
    [HttpGet("features")]
    public IActionResult Features()
    {
        return View();
    }
}