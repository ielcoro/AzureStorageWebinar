﻿using Microsoft.WindowsAzure.Storage;
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

        private CloudBlobContainer GetContainerAndCreateIfNeeded()
        {
            var containerReference = blobClient.GetContainerReference("userimages");

            containerReference.CreateIfNotExists();

            return containerReference;
        }

        public Uri SaveFile(string partitionKey, string rowKey, string fileName, Stream file)
        {
            CloudBlobContainer containerReference = GetContainerAndCreateIfNeeded();

            CloudBlobDirectory directory = containerReference.GetDirectoryReference(partitionKey + "/" + rowKey);

            CloudBlockBlob blockBlob = directory.GetBlockBlobReference(Path.GetFileName(fileName));

            if (blockBlob.Exists())
                blockBlob.CreateSnapshot();

            using (file)
            {
                blockBlob.UploadFromStream(file);
            }

            return blockBlob.Uri;
        }

        public IEnumerable<Uri> GetSnapshots(string partitionKey, string rowKey, string fileName)
        {
            CloudBlobContainer containerReference = GetContainerAndCreateIfNeeded();

            var blobsWithSnapshots = containerReference.ListBlobs(partitionKey + "/" + rowKey, true, BlobListingDetails.Snapshots);

            return blobsWithSnapshots
                        .Cast<CloudBlockBlob>()
                        .Where(b => b.IsSnapshot)
                        .Select(b => b.SnapshotQualifiedUri);
        }
    }
}
