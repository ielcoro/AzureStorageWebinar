using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UserPictureStorer.Repositories
{
    class ImageRepository
    {
        CloudStorageAccount cloudStorageAccount;
        CloudBlobClient blobClient;

        public ImageRepository()
        {
            this.cloudStorageAccount = CloudStorageAccount.Parse(Properties.Settings.Default.DataConnection);
            this.blobClient = cloudStorageAccount.CreateCloudBlobClient();
        }


        public Uri SaveTempFile(Stream stream)
        {
            CloudBlobContainer containerReference = GetContainerAndCreateIfNeeded();

            CloudBlobDirectory directory = containerReference.GetDirectoryReference("tempFiles");

            CloudBlockBlob blockBlob = directory.GetBlockBlobReference(Guid.NewGuid().ToString());

            using (stream)
            {
                blockBlob.UploadFromStream(stream);
            }

            return blockBlob.Uri;
        }

        private CloudBlobContainer GetContainerAndCreateIfNeeded()
        {
            var containerReference = blobClient.GetContainerReference("userImages");

            containerReference.CreateIfNotExists();

            return containerReference;
        }
    }
}
