using Restaurant.BusinessLogic.Base;
using Restaurant.Common.Extensions;
using Restaurant.Entities;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Restaurant.Common.DTOs;
using Restaurant.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Table = Restaurant.Entities.Table;
using Restaurant.BusinessLogic.Implementation.Tables.Models;

namespace Restaurant.BusinessLogic.Implementation.Users
{
    public class UserService : BaseService
    {
        private readonly RegisterValidator RegisterValidator;
        public UserService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
            RegisterValidator = new RegisterValidator(UnitOfWork);
        }

        public void RegisterUser(RegisterModel model)
        {
            RegisterValidator.Validate(model).ThenThrow(model);

            var user = Mapper.Map<RegisterModel, User>(model);

            var hashedPassword = PasswordHash(user.Password, user.PasswordHash);

            user.Password = hashedPassword;

            UnitOfWork.Users.Insert(user);
            UnitOfWork.SaveChanges();
        }

        public async Task<CurrentUserDTO> Login(string email, string password)
        {

            var user = await UnitOfWork.Users.Get()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return new CurrentUserDTO { IsAuthenticated = false };
            }

            var hashedPassword = PasswordHash(password, user.PasswordHash);

            if (hashedPassword == user.Password)
            {
                var currentUser = new CurrentUserDTO()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = $"{user.FirstName} {user.LastName}",
                    IsAuthenticated = true,
                    Role = Enum.GetName(typeof(RoleTypes), user.RoleId),
                };

                return currentUser;
            }

            return new CurrentUserDTO { IsAuthenticated = false };
        }


        private string PasswordHash(string password, Guid passwordHash)
        {
            var salt = passwordHash.ToString();
            SHA256 sha256Hash = SHA256.Create();
            byte[] computedHash = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            var hashedPasswordToString = Encoding.UTF8.GetString(computedHash);
            return hashedPasswordToString;
        }

    }
}
