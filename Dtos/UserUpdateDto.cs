using Microsoft.AspNetCore.Http;

namespace NomoBucket.API.Dtos
{
    public class UserUpdateDto
    {
        public string About { get; set; }
        public string Goals { get; set; }
        public string Country { get; set; }
        
        public IFormFile File {get; set;}
        public string PhotoUrl {get; set;}
        public string PublicPhotoId { get; set; }



    }
}