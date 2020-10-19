using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.API.Model;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;

namespace Hotel.API.Services
{
  public class ImageService : IImageService
  {
    private readonly IConfiguration _configuration;

    public ImageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Upload(IEnumerable<Images> imageList)
    {
      string storageConnectionString = _configuration.GetValue("storageConnectionString", "");

      // Check whether the connection string can be parsed.
      CloudStorageAccount storageAccount;

      CloudStorageAccount.TryParse(storageConnectionString, out storageAccount);

      // Create the CloudBlobClient that represents the 
      // Blob storage endpoint for the storage account.
      CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

      // Create a container called 'quickstartblobs' and 
      CloudBlobContainer cloudBlobContainer =
          cloudBlobClient.GetContainerReference("images");

      await cloudBlobContainer.CreateIfNotExistsAsync();

      // Set the permissions so the blobs are public.
      BlobContainerPermissions permissions = new BlobContainerPermissions
      {
        PublicAccess = BlobContainerPublicAccessType.Blob
      };

      await cloudBlobContainer.SetPermissionsAsync(permissions);


        var url = "";
        foreach (var item in imageList)
        {
            try
            {
                url = "";
                if (item.ImageType == (int) ImageType.HotelImage)
                {
                    url = "Hotel/" + item.HotelImageId;
                } 
                else 
                {
                    url = "Room/" + item.HotelImageId;
                }

                //url += item.TransactionId + "/" + item.Name;

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(url);

                if (item.Image.Length > 0)
                {
                    using (var stream = item.Image.OpenReadStream())
                    {
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    }
                }
            }
            catch(Exception ex)
            {
                //** If file name already exists then appent (1) with the name and upload it again...
                if (ex.Message.Contains("Conflict"))
                {
                    url += "/" + item.Name + "(1)";

                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(url);

                    if (item.Image.Length > 0)
                    {
                        using (var stream = item.Image.OpenReadStream())
                        {
                            await cloudBlockBlob.UploadFromStreamAsync(stream);
                        }
                    }
                }
            }
        }
    }
  }
}