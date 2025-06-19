using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class UserModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }
        public required string Role { get; set; }

        public static UserModel CreateModel(UserModel model, string password, string role) 
        {
            return new UserModel()
            {
                Id = Guid.NewGuid(),
                FullName = model.FullName,
                Email = model.Email,
                Password = password,
                Role = role
            };
        }
    }
}