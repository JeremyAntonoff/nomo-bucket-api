using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NomoBucket.API.Config;

namespace NomoBucket.API.Helpers
{
  public class CloudinaryHelper
  {
    private readonly IOptions<CloudinaryConfig> _cloudinaryConfig;
    private Cloudinary _cloudinary;
    public CloudinaryHelper(IOptions<CloudinaryConfig> cloudinaryConfig)
    
    {
      this._cloudinaryConfig = cloudinaryConfig;
      Account cloudinaryAccount = new Account( 
      
         _cloudinaryConfig.Value.CloudName,
         _cloudinaryConfig.Value.ApiKey,
         _cloudinaryConfig.Value.ApiSecret
      );
      _cloudinary = new Cloudinary(cloudinaryAccount);

    }
    public object UploadFile(IFormFile file) {
        var upload = new ImageUploadResult();
        using (var stream = file.OpenReadStream()) 
        {
            var options = new ImageUploadParams()
            {
                File= new FileDescription(file.Name, stream)
            };
            upload = _cloudinary.Upload(options);
        }
        return new {PhotoUrl = upload.Uri.ToString(), PublicPhotoId = upload.PublicId };
    }
    public bool DeleteFile(string photoId) {
        var photoDeletionParams = new DeletionParams(photoId);
        var photoDeletionResult = _cloudinary.Destroy(photoDeletionParams);
        if (photoDeletionResult.Result == "ok") {
            return true;
        }
        return false;
    }
  }
}