using System.Collections.Generic;
using System.Threading.Tasks;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data
{
    public interface IUserRepository
    {
         Task<User> getUser(int userId);
         Task<IEnumerable<User>> getUsers();
         Task<bool> saveAll();
    }
}