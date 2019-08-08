using System.Threading.Tasks;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> CheckExistingUser(string username);
    }
}