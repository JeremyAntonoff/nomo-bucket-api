using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nomo_bucket_api.Data.interfaces;
using nomo_bucket_api.Models;
using NomoBucket.API.Helpers;
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

        public async Task<PagedList<FeedItem>> GetFeed(FeedParams feedParams, int userId)
        {

            var feed = _context.FeedItems.OrderByDescending(l => l.CompletedAt);
            if (feedParams.Following == true)
            {
                var listOfFollowingIds = await this.getFollowing(userId);
                var followingFeed = feed.Where(feedItem => listOfFollowingIds.Contains(feedItem.UserId));
                return await PagedList<FeedItem>.CreatePagedList(followingFeed, feedParams.PageNumber, feedParams.PageSize);

            }
            var feedList = await PagedList<FeedItem>.CreatePagedList(feed, feedParams.PageNumber, feedParams.PageSize);
            return feedList;
        }
        private async Task<List<int>> getFollowing(int userId)
        {
            var ids = new List<int>();
            var followingUserIds = await _context.Follows.Where(f => f.FollowerId == userId).ToListAsync();
            followingUserIds.ForEach(u => ids.Add(u.FolloweeId));
            return ids;
        }


        public async Task<bool> SaveAll()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
    }
}