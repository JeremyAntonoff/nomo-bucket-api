using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nomo_bucket_api.Data.interfaces;
using NomoBucket.API.Data;

namespace nomo_bucket_api.Controllers {
    [Authorize]
    [ApiController]
    [Route ("/api/users/{userId}/[controller]")]
    public class BucketListController : ControllerBase {
        private readonly IBucketListRepository _bucketListRepo;
        private readonly IUserRepository _userRepo;
        public BucketListController (IBucketListRepository bucketListRepo, IUserRepository userRepo) {
            _bucketListRepo = bucketListRepo;
            _userRepo = userRepo;

        }

        [HttpGet]
        public async Task<IActionResult> GetBucketList (int userId) {
            var user = await _userRepo.GetUser(userId);
            if (user == null) {
                return NotFound();
            }
            return Ok(user.BucketList);
        }

    }
}