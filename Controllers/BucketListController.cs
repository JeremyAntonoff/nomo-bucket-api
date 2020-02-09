using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using nomo_bucket_api.Data.interfaces;
using NomoBucket.API.Config;
using NomoBucket.API.Data;
using NomoBucket.API.Dtos;
using NomoBucket.API.Helpers;
using NomoBucket.API.Models;

namespace nomo_bucket_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/users/{userId}/[controller]")]
    public class BucketListController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBucketListRepository _bucketListRepo;
        private readonly IUserRepository _userRepo;
        private readonly IOptions<CloudinaryConfig> _cloudinaryConfig;
        private readonly IFeedRepository _feedRepo;

        public BucketListController(IMapper mapper, IBucketListRepository bucketListRepo, IUserRepository userRepo, IFeedRepository feedRepo, IOptions<CloudinaryConfig> cloudinaryConfig)
        {
            _feedRepo = feedRepo;
            _mapper = mapper;
            _bucketListRepo = bucketListRepo;
            _userRepo = userRepo;
            _cloudinaryConfig = cloudinaryConfig;
        }

        [HttpGet]
        public async Task<IActionResult> GetBucketList(int userId)
        {
            var user = await _userRepo.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }
            var bucketListToReturn = _mapper.Map<IEnumerable<BucketListItemDto>>(user.BucketList);
            return Ok(bucketListToReturn);
        }
        [HttpPost]
        public async Task<IActionResult> AddBucketListItem(BucketListItemCreationDto bucketListItemCreationDto, int userId)
        {
            var userIdFromToken = this.User.Claims.First(c => c.Type == "userId").Value;
            if (int.Parse(userIdFromToken) != userId)
            {
                return Unauthorized();
            }
            var user = await _userRepo.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }
            if (bucketListItemCreationDto.description == null)
            {
                return BadRequest("Description is required!");
            }
            var bucketListItemToCreate = _mapper.Map<BucketListItem>(bucketListItemCreationDto);
            user.BucketList.Add(bucketListItemToCreate);

            if (await _userRepo.SaveAll())
            {
                return Ok(bucketListItemToCreate);
            }
            else
            {
                throw new Exception("Something went wrong adding the bucket list item!");
            }
        }

        [HttpPut("{bucketListId}")]
        public async Task<IActionResult> EditBucketListItem(int userId, int bucketListId, [FromForm]BucketListItemEditDto bucketListItemToEdit)
        {
            var userIdFromToken = this.User.Claims.First(c => c.Type == "userId").Value;
            if (int.Parse(userIdFromToken) != userId)
            {
                return Unauthorized();
            }
            var cloudinaryHelper = new CloudinaryHelper(_cloudinaryConfig);
            var user = await _userRepo.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }
            var bucketListItem = user.BucketList.FirstOrDefault(bl => bl.Id == bucketListId);
            if (bucketListItem == null)
            {
                return NotFound();
            }

            if (bucketListItemToEdit.File == null)
            {
                return BadRequest("No photo was submitted!");
            }
            dynamic photoUploadResults = cloudinaryHelper.UploadFile(bucketListItemToEdit.File);
            bucketListItem.CompletedPhotoUrl = photoUploadResults.PhotoUrl;
            bucketListItem.PublicPhotoId = photoUploadResults.PublicPhotoId;
            bucketListItem.Completed = true;
            bucketListItem.CompletedAt = DateTime.Now;
            bucketListItem.PhotoCaption = bucketListItemToEdit.photoCaption;

            var mappedFeed = _mapper.Map<FeedItem>(bucketListItem);
            mappedFeed.UserId = user.Id;
            mappedFeed.Username = user.Username;
            mappedFeed.ItemCreatedAt = bucketListItem.CreatedAt;
            mappedFeed.Category = "ITEM_COMPLETE";
            await _feedRepo.Add(mappedFeed);
            if (await _userRepo.SaveAll())
            {
                var userToReturn = _mapper.Map<BucketListItemDto>(bucketListItem);
                return Ok(bucketListItem);
            }
            throw new Exception($"Updating user with id {userId}'s bucketList failed");
        }
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem(int userId, int itemId)
        {
            var userIdFromToken = this.User.Claims.First(c => c.Type == "userId").Value;
            if (int.Parse(userIdFromToken) != userId)
            {
                return Unauthorized();
            }
            var user = await _userRepo.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }
            var bucketListItem = user.BucketList.FirstOrDefault(b => b.Id == itemId);
            if (bucketListItem != null)
            {
                user.BucketList.Remove(bucketListItem);
                if (bucketListItem.Completed == true)
                {
                    var deleted = await _feedRepo.DeleteFeedItemByPhotoUrl(bucketListItem.CompletedPhotoUrl);
                    var cloudinaryHelper = new CloudinaryHelper(_cloudinaryConfig);
                    cloudinaryHelper.DeleteFile(bucketListItem.PublicPhotoId);

                    if (deleted == false)
                    {
                        throw new Exception("Could not delete item from feed");
                    }
                }
                await _userRepo.SaveAll();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
            throw new Exception("Could not delete bucket list item");
        }
    }
}