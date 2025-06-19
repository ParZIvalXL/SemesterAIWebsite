using DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using WebWritterAI.Services;

namespace WebWritterAI.Controllers.Services;

[Controller]
[Route("[controller]")]
public class BlogController : Controller
{
    private readonly BlogService _blogService;
    private int _blogsPerPage = 9;
    
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
    public async Task<IActionResult> Blogs(int page, string search)
    {
        var blogs = await _blogService.GetBlogs();
        if (!string.IsNullOrEmpty(search))
            blogs = blogs.Where(x => x.Name.Contains(search)).ToList();
        int maxPage = (blogs.Count % _blogsPerPage);
        if(blogs.Count % _blogsPerPage > page)
            blogs = blogs.Skip(page * _blogsPerPage).Take(_blogsPerPage).ToList();
        return View("blog", new Tuple<IEnumerable<BlogModel>, int, int, string>(blogs, page, maxPage, search));
    }
}