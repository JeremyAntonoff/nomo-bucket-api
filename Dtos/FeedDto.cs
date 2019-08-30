using System;

namespace NomoBucket.API.Dtos
{
    public class FeedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public DateTime ItemCreatedAt { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string CompletedPhotoUrl { get; set; }
        public string PhotoCaption { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}