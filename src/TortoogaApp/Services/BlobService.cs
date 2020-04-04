using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.Services
{
    public class BlobService :IBlobService
    {

        public CloudBlobContainer GetImageBlobContainer(string containerName,string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();


            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // Create the container if it doesn't already exist.   
            container.CreateIfNotExistsAsync();

            container.SetPermissionsAsync(
            new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            return container;
        }

        public async void SaveImageToBlob(CloudBlobContainer container, byte[] bytes, string ImageName)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(ImageName);

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = new MemoryStream(bytes))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
        }

       
    }
}
