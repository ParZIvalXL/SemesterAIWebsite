using AutoMapper;
using WebWritterAI.Contracts.Contracts;
using DataBase.Models;

namespace WebWritterAI.Services.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {
            this.CreateMap<LoginContract, UserModel>();
            this.CreateMap<RegitsterContract, UserModel>();
        }
    }
}