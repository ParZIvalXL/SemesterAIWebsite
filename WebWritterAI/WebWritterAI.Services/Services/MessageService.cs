using DataBase.Models;
using DataBase.Repository;

namespace WebWritterAI.Services.Services;

public class MessageService
{
    private readonly MessageRepository _messageRepository;
    
    public MessageService(MessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
    public async Task<List<MessageModel>> GetMessages() => await _messageRepository.GetMessages();
    
    public async Task<List<MessageModel>> GetMessageByChatId(Guid id) => await _messageRepository.GetMessagesByChatId(id);
    
    public async Task AddMessage(MessageModel message) => await _messageRepository.CreateMessage(message);
    
}