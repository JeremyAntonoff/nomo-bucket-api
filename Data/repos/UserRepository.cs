using System.Collections.Generic;
using System.Linq;
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
            var users = await _context.Users.Include(u => u.BucketList).ToListAsync();
            return users;
        }
        public async Task<Follow> GetFollow(int followerId, int followeeId)
        {
            var follow = await _context.Follows.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
            return follow;
        }

        public async Task AddFollow(Follow follow)
        {
            await _context.Follows.AddAsync(follow);
        }
        public void RemoveFollow(Follow follow)
        {
            _context.Follows.Remove(follow);
        }
        public async Task<bool> SaveAll()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<IEnumerable<int>> GetFollowsForUser(int id)
        {
            var follows = await _context.Follows.Where(f => f.FollowerId == id).Select(col => col.FolloweeId).ToListAsync();
            return follows;
        }
    }
}