using System.Collections.Generic;
using System.Threading.Tasks;
using NomoBucket.API.Models;

namespace nomo_bucket_api.Data.interfaces {
    public interface IFeedRepository {
        Task<IEnumerable<FeedItem>> GetFeed ();
        Task<FeedItem> Add (FeedItem feedItem);
        Task<bool> SaveAll ();

    }
}