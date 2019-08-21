using System.Collections.Generic;
using System.Threading.Tasks;
using NomoBucket.API.Models;

namespace nomo_bucket_api.Data.interfaces {
    public interface IBucketListRepository {
        Task<IEnumerable<BucketListItem>> GetBucketList (int userId);
        BucketListItem GetBucketListItem (int userId, int bucketListItem);
        Task<bool> SaveAll ();

    }
}