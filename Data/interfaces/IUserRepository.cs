using System.Collections.Generic;
using System.Threading.Tasks;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data
{
    public interface IUserRepository
    {
         Task<User> GetUser(int userId);
         Task<IEnumerable<User>> GetUsers();
         Task<bool> SaveAll();
    }
}