using DataBase.Models;
using DataBase.Repository;

namespace WebWritterAI.Services;

public class PricingService
{
    private readonly PricingRepository _pricingRepository;
    
    public PricingService(PricingRepository pricingRepository)
    {
        _pricingRepository = pricingRepository;
    }
    
    public async Task<List<PricingModel>> GetPricings() => await _pricingRepository.GetPricings();
    
    public async Task<PricingModel?> GetPricing(Guid id) => await _pricingRepository.GetPricing(id);
}