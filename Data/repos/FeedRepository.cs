using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nomo_bucket_api.Data.interfaces;
using NomoBucket.API.Models;

namespace NomoBucket.API.Data.repos
{
  
    public class FeedRepository : IFeedRepository
    {
        private readonly DataContext _context;
        public FeedRepository(DataContext context)
        {
            _context = context;

        }

        public async Task<FeedItem> Add(FeedItem feedItem)
        {
        await _context.AddAsync(feedItem);
        return feedItem;
        }

    public async Task<IEnumerable<FeedItem>> GetFeed()
        {
        var feed = await _context.FeedItems.ToListAsync();
        return feed;
        }

        public async Task<bool> SaveAll()
        {
        var changes = await _context.SaveChangesAsync();
        return changes > 0;
        }
    }
}