using Microsoft.AspNetCore.Http;

namespace NomoBucket.API.Dtos
{
    public class BucketListItemEditDto
    {
        public string photoCaption { get; set; }
        public IFormFile File { get; set; }

    }
}