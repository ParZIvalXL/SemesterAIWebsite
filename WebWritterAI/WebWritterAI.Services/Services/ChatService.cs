using DataBase.Models;
using DataBase.Repository;

namespace WebWritterAI.Services.Services;

public class ChatService
{
    private readonly ChatRepository _repository;
    
    public ChatService(ChatRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<List<ChatModel>> GetChats() => await _repository.GetChats();
    
    public async Task<ChatModel> GetChat(Guid id) => await _repository.GetChat(id);
    
    public async Task AddChat(ChatModel chat) => await _repository.AddChat(chat);
    
    public async Task<bool> ChatExists(Guid id) => await _repository.ChatExists(id);
    
    public async Task<List<ChatModel>> GetUserChats(Guid id) => await _repository.GetUserChats(id);
}