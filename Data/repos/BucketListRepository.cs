using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nomo_bucket_api.Data.interfaces;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data
{
    public class BucketListRepository : IBucketListRepository
    {
        private readonly DataContext _context;
        public BucketListRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<BucketListItem>> GetBucketList(int userId)
        {
            var bucketList = await _context.BucketListItems.Where(u => u.Id == userId).ToListAsync();
            return bucketList;

        }

        public BucketListItem GetBucketListItem(int userId, int bucketListItemId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SaveAll()
        {
            throw new System.NotImplementedException();
        }
    }
}