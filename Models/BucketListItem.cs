using System;

namespace NomoBucket.API.Models
{
    public class BucketListItem
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string CompletedPhotoUrl { get; set; }
        public string PublicPhotoId { get; set; }
        public bool Completed { get; set; }
        public DateTime CompletedAt { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

    }
}