using System.Collections.Generic;
using System.Threading.Tasks;
using NomoBucket.API.Helpers;
using NomoBucket.API.Models;

namespace nomo_bucket_api.Data.interfaces
{
    public interface IFeedRepository
    {
        Task<PagedList<FeedItem>> GetFeed(FeedParams feedParams, int userId);
        Task<bool> DeleteFeedItemByPhotoUrl(string photoUrl);
        Task<FeedItem> Add(FeedItem feedItem);
        Task<bool> SaveAll();

    }
}