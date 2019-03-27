using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
///using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Queue; // Namespace for Queue storage types
using ImagesWebApi.Interfaces;

namespace ImagesWebApi.Services
{
    public class QueueService : IQueueService
    {
        private readonly string _queuecontainerName = "messages-queue";

        private readonly CloudQueueClient _queueClient;

        public QueueService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            _queueClient = storageAccount.CreateCloudQueueClient();
        }

        public async Task WriteMessageToQueueAsync(string message)
        {
            CloudQueue queue = _queueClient.GetQueueReference(_queuecontainerName);

            await queue.CreateIfNotExistsAsync();

            CloudQueueMessage cloudMessage = new CloudQueueMessage(message);

            await queue.AddMessageAsync(cloudMessage);
        }
    }
}
