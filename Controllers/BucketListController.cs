using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nomo_bucket_api.Data.interfaces;

namespace nomo_bucket_api.Controllers {
    [Authorize]
    [ApiController]
    [Route ("/users/{userId}/[controller]")]
    public class BucketListController : ControllerBase {
        private readonly IBucketListRepository _repo;
        public BucketListController (IBucketListRepository repo) {
            _repo = repo;

        }

        [HttpGet]
        public async Task<IActionResult> GetBucketList (int userId) {
            var bucketList = await _repo.GetBucketList(userId);
            if (bucketList == null) {
                return NotFound();
            }
            return Ok(bucketList);
        }

    }
}