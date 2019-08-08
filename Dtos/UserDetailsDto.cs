using System;
using System.Collections.Generic;
using NomoBucket.API.Models;

namespace NomoBucket.API.Dtos
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string About { get; set; }
        public string Goals { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public DateTime LastActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Country { get; set; } 
        public string PhotoUrl { get; set; }
        public ICollection<BucketListItemDto> BucketList { get; set; }
        
    }
}