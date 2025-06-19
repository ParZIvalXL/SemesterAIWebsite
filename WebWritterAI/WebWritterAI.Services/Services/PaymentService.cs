using DataBase.Models;
using DataBase.Repository;

namespace WebWritterAI.Services.Services;

public class PaymentService
{
    private readonly PaymentRepository _paymentRepository;

    public PaymentService(PaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }
    
    public async Task<List<PaymentInfoModel>> GetPayments() => await _paymentRepository.GetPayments();
    
    public async Task<PaymentInfoModel?> GetPayment(Guid id) => await _paymentRepository.GetPayment(id);
    
    public async Task CreatePayment(PaymentInfoModel paymentInfoModel) => await _paymentRepository.CreatePayment(paymentInfoModel);
    public async Task<List<PaymentInfoModel>> GetAllUserPayments(Guid id) => await _paymentRepository.GetAllUserPayments(id);
}