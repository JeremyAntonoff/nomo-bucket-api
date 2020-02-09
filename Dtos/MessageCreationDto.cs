using System;

namespace nomo_bucket_api.Dtos
{
    public class MessageCreationDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public MessageCreationDto()
        {
            this.CreatedAt = DateTime.Now;
        }

    }
}