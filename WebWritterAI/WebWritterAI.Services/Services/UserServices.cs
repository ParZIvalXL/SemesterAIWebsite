using System.Security.Claims;
using AutoMapper;
using WebWritterAI.Contracts.Contracts;
using DataBase.Models;
using FastkartAPI.DataBase.Repositories;
using FastkartAPI.Infrastructure.Password;
using WebWriterAI.Infrastructure.Password;

namespace WebWritterAI.Services.Services
{
    public class UserService
    {
        private readonly UserModelRepository _userModelRepository;
        private readonly JwtProvider _jwtProvider;
        private readonly PasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(
            UserModelRepository repository,
            JwtProvider jwtProvider,
            PasswordHasher passwordHasher,
            IMapper mapper) 
        {
            _userModelRepository = repository;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task Register(RegitsterContract regitster) 
        {

            var user = _mapper.Map<UserModel>(regitster);

            var password = _passwordHasher.GenerateTokenSHA(user.Password);

            await _userModelRepository.Create(user, password, "user");
        }

        public async Task<int> GetCount()
        {
            return await _userModelRepository.GetCount();
        }
        
        public async Task<string> Login(LoginContract login)
        {
            var user = await _userModelRepository.GetByEmail(login.Email);
            
            if (user == null) throw new Exception("Такого пользователя с почтой не существует");
            
            var result = _passwordHasher.Verify(login.Password, user.Password);

            if (!result) throw new Exception("Неверный пароль");

            var token = _jwtProvider.GenerateToken(user);
            
            return token;
        }


        public async Task Delete(Guid id)
        {
            var user = await _userModelRepository.GetById(id);
            if (user == null) throw new Exception("Такого пользователя не существует");
            if(user.Role == "admin") throw new Exception("Нельзя удалить администратора");
            await _userModelRepository.Delete(id);
        }
        
        public async Task Update(UserModel user) 
        {
            await _userModelRepository.Update(user);
        }

        public async Task Remove(Guid id) 
        {
            await _userModelRepository.Delete(id);
        }

        public async Task<List<UserModel>> GetAll() 
        {
            return await _userModelRepository.GetAll();
        }

        public async Task<UserModel> GetById(Guid id) 
        {
            return await _userModelRepository.GetById(id);
        }

        public async Task<UserModel?> GetByEmail(string email) 
        {
            return await _userModelRepository.GetByEmail(email);
        }
        
        public async Task<UserModel?> GetByName(string name) 
        {
            return await _userModelRepository.GetByName(name);
        }
        
        public async Task<Guid?> GetUserByToken(ClaimsPrincipal claims)
        {
            var userIdClaim = claims.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return null; // Возвращаем null, если нет userId или он некорректен
            }

            return userId; // Возвращаем корректный userId
        }

        public async Task<UserModel?> CancelSubscription(Guid idVal)
        {
            var user = await _userModelRepository.GetById(idVal);
            user.Role = "user";
            await _userModelRepository.Update(user);
            return user;
        }

        public async Task<UserModel?> SetSubscription(Guid userVal, Guid idVal)
        {
            var user = await _userModelRepository.GetById(userVal);
            user.Role = idVal.ToString();
            await _userModelRepository.Update(user);
            return user;
        }
    }
}