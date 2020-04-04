using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TortoogaApp.Services
{
    public interface IBlobService
    {
        CloudBlobContainer GetImageBlobContainer(string containerName, string connectionString);

        void SaveImageToBlob(CloudBlobContainer container, byte[] bytes, string ImageName);       


    }
}
