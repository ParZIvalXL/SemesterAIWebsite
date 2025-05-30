using Microsoft.AspNetCore.Mvc;
using WebWritterAI.Services;

namespace WebWritterAI.Controllers.Services;

[Controller]
[Route("[controller]")]
public class BlogController : Controller
{
    private readonly BlogService _blogService;
    
    public BlogController(BlogService blogService)
    {
        _blogService = blogService;
    }
    
    [HttpGet("blog")]
    public async Task<IActionResult> Index([FromBody]Guid id)
    {
        var blog = await _blogService.GetBlog(id);    
        return View("blog-single",blog);
    }
    
    [HttpGet("blogs")]
    public async Task<IActionResult> Blogs()
    {
        var blogs = await _blogService.GetBlogs();    
        return Ok(blogs);
    }
}