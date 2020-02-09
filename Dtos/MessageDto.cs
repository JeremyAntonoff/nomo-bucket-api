using System;

namespace nomo_bucket_api.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverUsername { get; set; }
        public string ReceiverPhotoUrl { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}