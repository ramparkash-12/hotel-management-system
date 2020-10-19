using System;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace Hotel.API.Extensions
{
  public static class ImageExtension
  {
    public static CloudBlobContainer GetStorageContainer(string storageConnStr)
    {
        try
        {
        string account = storageConnStr;
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(account);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = blobClient.GetContainerReference("images");

        return container;
        }
        catch (Exception)
        {
        return null;
        }
    }

    public static string GenerateURI(int ImageType, int? ImageReferenceId, string Imagename)
    {
        string url = "";

        if (ImageType == 1)
        {
            url = "Hotel/" + ImageReferenceId +  "/" + Imagename;
        } 
        else if (ImageType == 2)
        {
            url = "Room/" + ImageReferenceId +  "/" + Imagename;
        }

        return url;
    }

  }
}