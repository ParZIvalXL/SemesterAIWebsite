using Microsoft.EntityFrameworkCore;
using DataBase.Models;

namespace DataBase.Repository;

public class MessageRepository
{
    private readonly ApplicationDBContext _context;
    
    public MessageRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<MessageModel>> GetMessages() => await _context.Messages.ToListAsync();
    
    public async Task<MessageModel?> GetMessage(int id) => await _context.Messages.FindAsync(id);
    
    public async Task CreateMessage(MessageModel message)
    {
        await _context.AddAsync(message);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<MessageModel>> GetMessagesByChatId(Guid id) => await _context.Messages.Where(x => x.ChatId == id).ToListAsync();
}