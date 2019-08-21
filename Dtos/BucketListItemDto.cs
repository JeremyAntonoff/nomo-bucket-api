using System;

namespace NomoBucket.API.Dtos
{
    public class BucketListItemDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string CompletedPhotoUrl { get; set; }
        public bool Completed { get; set; }
        public DateTime CompletedAt { get; set; }

        
    }
}