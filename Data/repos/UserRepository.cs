using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nomo_bucket_api.Models;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            this._context = context;
        }
        public async Task<User> GetUser(int userId)
        {
            var foundUser = await _context.Users.Include(u => u.BucketList).FirstOrDefaultAsync(u => u.Id == userId);
            return foundUser;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }
        public async Task<bool> doesFollowExist(int followerId, int followeeId)
        {
            var follow = await _context.Follows.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
            if (follow != null)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        public async Task AddFollow(Follow follow)
        {
            await _context.AddAsync(follow);
        }
        public async Task<bool> SaveAll()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
    }
}