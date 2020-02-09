using System.Collections.Generic;
using System.Threading.Tasks;
using nomo_bucket_api.Helpers;
using nomo_bucket_api.Models;
using NomoBucket.API.Helpers;

namespace nomo_bucket_api.Data.interfaces
{
    public interface IMessageRepository
    {
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetUserMessages(int userId, MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
        Task<Message> Add(Message message);
        Task<bool> SaveAll();

    }
}