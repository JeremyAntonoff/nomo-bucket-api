using System;

namespace NomoBucket.API.Dtos
{
    public class BucketListItemCreationDto
    {
        public string description { get; set; }
        public DateTime CreatedAt { get; set; }
        public BucketListItemCreationDto() 
        {
            CreatedAt = DateTime.Now;
        }
    }
}