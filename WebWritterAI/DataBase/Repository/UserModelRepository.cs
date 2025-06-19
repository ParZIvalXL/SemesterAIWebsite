using DataBase;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace FastkartAPI.DataBase.Repositories
{
    public class UserModelRepository
    {
        private readonly ApplicationDBContext _context;

        public UserModelRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task Create(UserModel user, string password, string role)
        {
            var result = UserModel.CreateModel(user, password, role);

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await _context.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<List<UserModel>> GetAll()
        {
            var result = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task<UserModel> GetById(Guid id)
        {
            var result = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<UserModel?> GetByEmail(string email) 
        {
            var result = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);

            return result;
        }
        
        public async Task<int> GetCount()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<UserModel?> GetByName(string name)
        {
            var result = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FullName == name);

            return result;
        }

        public Task Update(UserModel user)
        {
            _context.Users.Update(user);
            return _context.SaveChangesAsync();
        }
    }
}