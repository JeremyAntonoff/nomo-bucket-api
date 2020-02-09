using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nomo_bucket_api.Data.interfaces;
using nomo_bucket_api.Helpers;
using nomo_bucket_api.Models;
using NomoBucket.API.Data;
using NomoBucket.API.Helpers;

namespace nomo_bucket_api.Data.repos
{
    public class MessageRepository : IMessageRepository
    {
        private DataContext _context;
        public MessageRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Message> GetMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            return message;
        }

        public async Task<Message> Add(Message message)
        {
            await _context.AddAsync(message);
            return message;
        }
        public async Task<bool> SaveAll()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<PagedList<Message>> GetUserMessages(int userId, MessageParams messageParams)
        {
            var messages = _context.Messages
            .Include(u => u.Sender)
            .Include(u => u.Receiver)
            .AsQueryable();

            switch (messageParams.MessageType)
            {
                case "received":
                    messages = messages.Where(m => m.ReceiverId == userId);
                    break;
                case "sent":
                    messages = messages.Where(m => m.SenderId == userId);
                    break;
                default:
                    messages = messages.Where(m => m.ReceiverId == userId);
                    break;
            }
            messages = messages.OrderByDescending(m => m.CreatedAt);
            return await PagedList<Message>.CreatePagedList(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
             var messages = await _context.Messages
            .Include(u => u.Sender)
            .Include(u => u.Receiver)
            .Where(m => (m.ReceiverId == userId && m.SenderId == recipientId) || (m.ReceiverId == recipientId && m.SenderId == userId))
            .ToListAsync();
            
            return messages.OrderByDescending(m => m.CreatedAt);
        }
        
    }
}