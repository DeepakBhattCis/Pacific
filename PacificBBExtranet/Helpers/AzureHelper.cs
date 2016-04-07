using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PacificBBExtranet.Web.Helpers.Azure
{
    public class AzureHelper
    {

        private readonly CloudBlobClient _cloudBlobClient;

        public AzureHelper(string connectString = null)
        {
            if (connectString != null)
            {
                _cloudBlobClient = CloudStorageAccount.Parse(connectString).CreateCloudBlobClient();
            }
            else
            {
                //UseDevelopmentStorage=true
                _cloudBlobClient = CloudStorageAccount.Parse("UseDevelopmentStorage=true").CreateCloudBlobClient();
            }
        }



        public string SaveToBlob(
          byte[] file,
          string blobContainer,
          string fileName,
          string ContentType = "image/jpg")
        {
            try
            {
                if (file != null)
                {
                    var blobStorage = _cloudBlobClient;
                    CloudBlobContainer container = blobStorage.GetContainerReference(blobContainer);
                    try
                    {
                        if (container.CreateIfNotExists())
                        {
                            // configure container for public access
                            var permissions = container.GetPermissions();
                            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                            container.SetPermissions(permissions);
                        }
                    }
                    catch (Exception ex)
                    {

                        var asd = 3;
                    }

                    CloudBlockBlob blob = blobStorage.GetContainerReference(blobContainer).GetBlockBlobReference(fileName);
                    //blob.Properties.ContentType = file.ContentType;
                    blob.Properties.ContentType = ContentType;
                    blob.Properties.CacheControl = "public, max-age=31536000";
                    blob.UploadFromByteArray(file, 0, file.Length);


                    return blob.Uri.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //quizas esto no deba ir aqui
        //protected string GetProfileImageUrl(string imageName)
        //{
        //    CloudBlockBlob blob;
        //    if (imageName != null)
        //        blob = GetFileFromStorage(BlobContainers.images, imageName);
        //    else
        //        blob = GetFileFromStorage(BlobContainers.images, "NoProfile.jpg");

        //    return blob.Uri.ToString();
        //}

        //private CloudBlockBlob GetFileFromStorage(string blobContainer, string blobName)
        //{
        //    if (!string.IsNullOrWhiteSpace(blobName))
        //    {
        //        var blobStorage = _cloudBlobClient;
        //        var container = blobStorage.GetContainerReference(blobContainer);
        //        CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
        //        return blob;
        //    }
        //    return null;
        //}


        //protected void RemoveProfileImage(string profileImageName = null)
        //{
        //    if (profileImageName != null)
        //        RemoveBlob(BlobContainers.images, profileImageName);
        //}

        //private void RemoveBlob(string container, string blobName)
        //{
        //    var blobStorage = _cloudBlobClient;
        //    var blobContainer = blobStorage.GetContainerReference(container);
        //    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);

        //    blob.DeleteIfExists();
        //}
    }
}