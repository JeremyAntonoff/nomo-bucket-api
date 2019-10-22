using System;
using System.Collections.Generic;
using nomo_bucket_api.Models;

namespace NomoBucket.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public string About { get; set; }
        public string Goals { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public string PublicPhotoId { get; set; }
        public ICollection<BucketListItem> BucketList { get; set; }
        public ICollection<Follow> Followers { get; set; }
        public ICollection<Follow> Followees { get; set; }

    }
}