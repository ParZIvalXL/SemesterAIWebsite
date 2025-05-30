using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository;

public class UseCaseRepository
{
    private readonly ApplicationDBContext _context;

    public UseCaseRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<UseCaseModel>> GetUseCases() => await _context.UseCases.ToListAsync();
    public async Task<UseCaseModel> GetUseCase(Guid id) => await _context.UseCases.Where(x => x.Id == id).FirstOrDefaultAsync();
}