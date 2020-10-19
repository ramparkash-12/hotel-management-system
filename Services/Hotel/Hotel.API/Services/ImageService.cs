using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.API.Extensions;
using Hotel.API.Model;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;

namespace Hotel.API.Services
{
  public class ImageService : IImageService
  {
    private readonly IConfiguration _configuration;
    private readonly string _storageConnectionString;

    public ImageService(IConfiguration configuration)
    {
        _configuration = configuration;
        _storageConnectionString = _configuration.GetValue("storageConnectionString", "");
    }

    public async Task Delete(IEnumerable<Images> imageList)
    {
        CloudBlobContainer container = ImageExtension.GetStorageContainer(_storageConnectionString);
        var url = "";

        if (container != null)
        {
            await container.CreateIfNotExistsAsync();
            
            foreach (var item in imageList)
            {
                try
                {
                    url = "";
                    if (item.ImageType == (int) ImageType.HotelImage)
                    {
                        url = ImageExtension.GenerateURI(1, item.HotelImageId, item.Name);
                    } 
                    else 
                    {
                        url = ImageExtension.GenerateURI(2, item.HotelImageId, item.Name);
                    }

                    CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(url);
                    cloudBlockBlob.Delete();
                
                }
                catch(Exception)
                {
                    CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(url);
                    cloudBlockBlob.Delete();
                }
            }
        }
        else
        {
            throw new Exception("Unable to get container reference");
        }
    }

    public async Task Upload(IEnumerable<Images> imageList)
    {
        CloudBlobContainer container = ImageExtension.GetStorageContainer(_storageConnectionString);
        var url = "";

        if (container != null)
        {
            await container.CreateIfNotExistsAsync();

            // Set the permissions so the blobs are public.
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };

            await container.SetPermissionsAsync(permissions);
            
            foreach (var item in imageList)
            {
                try
                {
                    url = "";
                    if (item.ImageType == (int) ImageType.HotelImage)
                    {
                        url = ImageExtension.GenerateURI(1, item.HotelImageId, item.Name);
                    } 
                    else 
                    {
                        url = ImageExtension.GenerateURI(2, item.HotelImageId, item.Name);
                    }

                    CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(url);

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
                    if (ex.Message.Contains("exists"))
                    {
                        url += "(1)";

                        CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(url);

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
        else
        {
            throw new Exception("Unable to get container reference");
        }
    }
  }
}