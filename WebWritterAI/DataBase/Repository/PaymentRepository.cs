using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository;

public class PaymentRepository
{
    private readonly ApplicationDBContext _context;

    public PaymentRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<PaymentInfoModel>> GetPayments() => await _context.PaymentInfos.ToListAsync();
    
    public async Task<PaymentInfoModel?> GetPayment(Guid id) => await _context.PaymentInfos.FirstOrDefaultAsync(x => x.Id == id);
    public async Task CreatePayment(PaymentInfoModel pricing) => await _context.PaymentInfos.AddAsync(pricing);
    public async Task<List<PaymentInfoModel>> GetAllUserPayments(Guid id) => await _context.PaymentInfos.Where(x => x.UserId == id.ToString()).ToListAsync();
}