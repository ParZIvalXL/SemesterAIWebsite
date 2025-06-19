using DataBase.Models;
using DataBase.Repository;

namespace WebWritterAI.Services;

public class BlogService
{
    private readonly BlogRepository _blogRepository;
    
    public BlogService(BlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }
    
    public async Task<List<BlogModel>> GetBlogs() => await _blogRepository.GetBlogs();
    
    public async Task<BlogModel> GetBlog(Guid id) => await _blogRepository.GetBlog(id);
}