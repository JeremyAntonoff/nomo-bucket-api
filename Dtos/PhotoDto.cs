using Microsoft.AspNetCore.Http;

namespace NomoBucket.API.Dtos
{
    public class PhotoDto
    {
        public IFormFile File { get; set; }
    }
}