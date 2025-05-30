using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository;

public class BlogRepository
{
    public ApplicationDBContext _context { get; set; }
    
    public BlogRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<BlogModel>> GetBlogs() => await _context.Blogs.ToListAsync();
    
    public async Task<BlogModel> GetBlog(Guid id) => await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
}