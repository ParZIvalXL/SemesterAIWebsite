using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository;

public class PricingRepository
{
    private readonly ApplicationDBContext _context;

    public PricingRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<PricingModel>> GetPricings() => await _context.Pricings.ToListAsync();
}