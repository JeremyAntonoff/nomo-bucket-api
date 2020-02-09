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
        Task<IEnumerable<int>> GetFollowsForUser(int id);
        Task<Follow> GetFollow(int followerId, int followeeId);
        Task AddFollow(Follow follow);
        void RemoveFollow(Follow follow);

        Task<bool> SaveAll();
    }
}