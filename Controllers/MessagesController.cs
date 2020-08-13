using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using nomo_bucket_api.Data.interfaces;
using nomo_bucket_api.Dtos;
using nomo_bucket_api.Helpers;
using nomo_bucket_api.Models;
using NomoBucket.API.Data;
using NomoBucket.API.Helpers;

namespace nomo_bucket_api.Controllers
{
    [ServiceFilter(typeof(UserActivity))]
    [ApiController]
    [Route("/api/users/{userId}/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessagesController(IMessageRepository messageRepo, IUserRepository userRepository, IMapper mapper)
        {
            _messageRepo = messageRepo;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserMessages(int userId, [FromQuery]MessageParams messageParams)
        {
            var userIdFromToken = this.User.Claims.First(c => c.Type == "userId").Value;
            if (int.Parse(userIdFromToken) != userId)
            {
                return Unauthorized();
            }
            var messages = await _messageRepo.GetUserMessages(userId, messageParams);
            var messagesToReturn = _mapper.Map<IEnumerable<MessageDto>>(messages);
            Response.AddPagination(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return Ok(messagesToReturn);
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            var userIdFromToken = this.User.Claims.First(c => c.Type == "userId").Value;
            if (int.Parse(userIdFromToken) != userId)
            {
                return Unauthorized();
            }

            var message = await _messageRepo.GetMessage(id);
            if (message == null)
            {
                return BadRequest();
            }
            return Ok(message);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            var userIdFromToken = this.User.Claims.First(c => c.Type == "userId").Value;
            if (int.Parse(userIdFromToken) != userId)
            {
                return Unauthorized();
            }

            var messages = await _messageRepo.GetMessageThread(userId, recipientId);
            foreach (var message in messages)
            {
                message.IsRead = true;
            }
            var messagesToReturn = _mapper.Map<IEnumerable<MessageDto>>(messages);

            return Ok(messagesToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageCreationDto messageForCreationDto)
        {
            var sender = await _userRepository.GetUser(userId);
            var userIdFromToken = this.User.Claims.First(c => c.Type == "userId").Value;
            if (int.Parse(userIdFromToken) != userId)
            {
                return Unauthorized();
            }
            messageForCreationDto.SenderId = userId;
            var receiverUserPresent = await _userRepository.GetUser(messageForCreationDto.ReceiverId);
            if (receiverUserPresent == null)
            {
                return BadRequest("Could not find receiver");
            }
            var messageToSave = _mapper.Map<Message>(messageForCreationDto);
            await _messageRepo.Add(messageToSave);
            var savedChanges = await _messageRepo.SaveAll();
            var messageToReturn = _mapper.Map<MessageDto>(messageToSave);

            if (savedChanges)
            {
                return CreatedAtRoute("GetMessage", new { id = messageToSave.Id, userId }, messageToReturn);
            }
            throw new Exception("Could not create message");
        }
    }

}