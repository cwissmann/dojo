using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_blob
{
    class Program
    {
        private static string accountName = "";
        private static string keyValue = "";

        private static string fileName = "nashorn.jpg";

        static void Main(string[] args)
        {
            //UploadBlob();

            ListBlobs();

            DeleteBlobs();

            Console.ReadLine();
        }

        private static void UploadBlob()
        {
            var storageCredentials = new StorageCredentials(accountName, keyValue);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);

            var blobClient = storageAccount.CreateCloudBlobClient();

            var containers = blobClient.ListContainers();

            CloudBlobContainer dojoContainer = blobClient.GetContainerReference("mycontainer1");

            CloudBlockBlob dojoBlob = dojoContainer.GetBlockBlobReference(fileName);

            using (var filestream = File.OpenRead(fileName))
            {
                dojoBlob.UploadFromStream(filestream);
            }
        }

        private static void ListBlobs()
        {
            var storageCredentials = new StorageCredentials(accountName, keyValue);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);

            var blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer dojoContainer = blobClient.GetContainerReference("mycontainer1");

            foreach (IListBlobItem item in dojoContainer.ListBlobs())
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);
                }
            }
        }

        private static void DeleteBlobs()
        {
            var storageCredentials = new StorageCredentials(accountName, keyValue);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);

            var blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer dojoContainer = blobClient.GetContainerReference("dojoContainer");

            CloudBlockBlob dojoBlob = dojoContainer.GetBlockBlobReference(fileName);

            //var sas = dojoBlob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            //{
            //    Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Delete | SharedAccessBlobPermissions.Read,
            //    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(1)
            //});

            dojoBlob.Delete();
        }
    }
}
