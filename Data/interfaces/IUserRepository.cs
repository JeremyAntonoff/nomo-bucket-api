using System.Collections.Generic;
using System.Threading.Tasks;
using nomo_bucket_api.Models;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data
{
    public interface IUserRepository
    {
         Task<User> GetUser(int userId);
         Task<IEnumerable<User>> GetUsers();

         Task<bool> doesFollowExist(int followerId, int followeeId);

         Task AddFollow(Follow follow);
         Task<bool> SaveAll();
    }
}