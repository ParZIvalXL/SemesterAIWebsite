using DataBase.Models;
using DataBase.Repository;

namespace WebWritterAI.Services;

public class UseCaseService
{
    private readonly UseCaseRepository _blockRepository;
    
    public UseCaseService(UseCaseRepository blockRepository)
    {
        _blockRepository = blockRepository;
    }
    
    public async Task<List<UseCaseModel>> GetUseCases() => await _blockRepository.GetUseCases();
    public async Task<UseCaseModel> GetUseCase(Guid id) => await _blockRepository.GetUseCase(id);
}