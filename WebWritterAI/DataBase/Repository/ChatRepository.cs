using DataBase.Models;
using Microsoft.EntityFrameworkCore;


namespace DataBase.Repository;

public class ChatRepository
{
    public ApplicationDBContext _context;
    public ChatRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<ChatModel>> GetChats() => await _context.Chats.ToListAsync();

    public async Task<ChatModel> GetChat(Guid id) => await _context.Chats.FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddChat(ChatModel chat)
    {
        await _context.AddAsync(chat);
        await _context.SaveChangesAsync();
        
    }
    
    public async Task<bool> ChatExists(Guid id) => await _context.Chats.AnyAsync(x => x.Id == id);
    
    public async Task<List<ChatModel>> GetUserChats(Guid id) => await _context.Chats.Where(x => x.UserId == id).ToListAsync();
}