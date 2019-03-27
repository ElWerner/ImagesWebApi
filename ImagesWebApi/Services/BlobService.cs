using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;
using ImagesWebApi.Interfaces;

namespace ImagesWebApi.Services
{
    public class BlobService : IBlobService
    {
        private readonly string _blobContainerName = "images-container";

        private readonly CloudBlobClient _blobClient;
        private readonly CloudStorageAccount _storageAccount;

        public BlobService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            if (CloudStorageAccount.TryParse(connectionString, out var storageAccount ))
            {
                _storageAccount = storageAccount;
                _blobClient = storageAccount.CreateCloudBlobClient();
            }
            else
            {
                throw new StorageException("Can't connect to the cloud storage account.");
            }
        }

        public async Task<byte[]> GetBlobByNameAsync(string blobName)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(_blobContainerName);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            await blockBlob.FetchAttributesAsync();

            byte[] blobByteArray = new byte[blockBlob.Properties.Length];
            await blockBlob.DownloadToByteArrayAsync(blobByteArray, 0);

            return blobByteArray;
        }

        public async Task UploadBlobToStorageAsync(Stream stream, string blobName)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(_blobContainerName);

            if (container == null)
            {
                container = await CreateBlobContainer(_blobContainerName, _storageAccount);
            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            await blockBlob.UploadFromStreamAsync(stream);
        }

        private async Task<CloudBlobContainer> CreateBlobContainer(string containerName, CloudStorageAccount storageAccount)
        {
            var cloudBlobContainer = _blobClient.GetContainerReference(containerName);

            await cloudBlobContainer.CreateAsync();

            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Container
            };

            await cloudBlobContainer.SetPermissionsAsync(permissions);

            return cloudBlobContainer;
        }
    }

}
