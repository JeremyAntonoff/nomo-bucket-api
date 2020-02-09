using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using nomo_bucket_api.Models;
using NomoBucket.API.Config;
using NomoBucket.API.Data;
using NomoBucket.API.Dtos;
using NomoBucket.API.Helpers;

namespace NomoBucket.API.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(UserActivity))]
    [Route("/api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        private readonly IOptions<CloudinaryConfig> _CloudinaryConfig;

        public UsersController(IUserRepository repo, IMapper mapper, IOptions<CloudinaryConfig> cloudinaryConfig)
        {
            this._repo = repo;
            this._mapper = mapper;
            this._CloudinaryConfig = cloudinaryConfig;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserListDto>>(users);
            return Ok(usersToReturn);
        }
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            var userToReturn = _mapper.Map<UserDetailsDto>(user);

            return Ok(userToReturn);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userToUpdate)
        {
            var userIdFromToken = this.User.Claims.First(c => c.Type == "userId").Value;
            if (int.Parse(userIdFromToken) != id)
            {
                return Unauthorized();
            }

            var repoUser = await _repo.GetUser(id);
            _mapper.Map(userToUpdate, repoUser);

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating user with ID: {id} Failed");
        }

        [HttpPut("{id}/photo")]
        public async Task<IActionResult> UpdateUserPhoto(int id, [FromForm]PhotoDto userPhotoToUpdate)
        {
            var userIdFromToken = this.User.Claims.First(c => c.Type == "userId").Value;
            if (int.Parse(userIdFromToken) != id)
            {
                return Unauthorized();
            }
            var cloudinaryHelper = new CloudinaryHelper(_CloudinaryConfig);
            var repoUser = await _repo.GetUser(id);

            if (userPhotoToUpdate.File == null)
            {
                return BadRequest("No photo was submitted!");
            }
            if (repoUser.PublicPhotoId != null)
            {
                var photoDeleted = cloudinaryHelper.DeleteFile(repoUser.PublicPhotoId);
                if (photoDeleted)
                {
                    repoUser.PhotoUrl = null;
                    repoUser.PublicPhotoId = null;
                }
                else
                {
                    throw new Exception("Error trying to delete photo from the cloud!");
                }
            }
            dynamic photoUploadResults = cloudinaryHelper.UploadFile(userPhotoToUpdate.File);
            repoUser.PhotoUrl = photoUploadResults.PhotoUrl;
            repoUser.PublicPhotoId = photoUploadResults.PublicPhotoId;
            if (await _repo.SaveAll())
            {
                var userToReturn = _mapper.Map<UserDetailsDto>(repoUser);
                return Ok(userToReturn);
            }
            throw new Exception($"Updating user photo with ID: {id} Failed");
        }
        [HttpGet("{id}/follows")]
        public async Task<IActionResult> GetFollows(int id, int followeeId)
        {
            var userIdFromToken = this.User.Claims.First(c => c.Type == "userId").Value;
            if (int.Parse(userIdFromToken) != id)
            {
                return Unauthorized();
            }
            var follows = await _repo.GetFollowsForUser(id);
            if (follows != null)
            {
                return Ok(follows);
            }
            return Ok(new int[0]);
        }
        [HttpPost("{id}/follow/{followeeId}")]
        public async Task<IActionResult> followUser(int id, int followeeId)
        {
            var userIdFromToken = this.User.Claims.First(c => c.Type == "userId").Value;
            if (int.Parse(userIdFromToken) != id)
            {
                return Unauthorized();
            }
            if (await _repo.GetUser(followeeId) == null)
            {
                return NotFound();
            }
            var followExists = await _repo.GetFollow(id, followeeId);
            if (followExists != null)
            {
                _repo.RemoveFollow(followExists);
            }
            else
            {
                var follow = new Follow();
                follow.FollowerId = id;
                follow.FolloweeId = followeeId;

                await _repo.AddFollow(follow);
            }

            if (await _repo.SaveAll())
            {
                return Ok();
            }
            return BadRequest("Failled to follow user");

        }
    }
}
