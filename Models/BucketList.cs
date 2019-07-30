using System;

namespace NomoBucket.API.Models
{
    public class BucketList
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Description { get; set; }
        public string CompletedPhotoUrl { get; set; }
        public bool Completed { get; set; }
        public DateTime CompletedAt { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }


    }
}