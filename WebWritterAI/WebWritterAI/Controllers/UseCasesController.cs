using Microsoft.AspNetCore.Mvc;
using WebWritterAI.Services;

namespace WebWritterAI.Controllers.Services;

[Controller]
[Route("api/[controller]")]
public class UseCasesController : Controller
{
    private readonly UseCaseService _useCase;
    
    public UseCasesController(UseCaseService useCase)
    {
        _useCase = useCase;
    }
    
    [HttpGet("usecase")]
    public async Task<IActionResult> Index([FromBody]Guid id)
    {
        var useCase = await _useCase.GetUseCase(id);
        return View("usecase-details", useCase);
    }
    
    [HttpGet("usecases")]
    public async Task<IActionResult> UseCases()
    {
        var useCases = await _useCase.GetUseCases();
        return View("usecase", useCases);
    }
}