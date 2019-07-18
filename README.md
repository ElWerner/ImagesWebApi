# ImagesWebApi
Create web API app with 2 endpoints:
* Get by id - download image with blur effect;
* Post - upload image.
                
On Post method image should be saved in Blob storage, then new message with image name should be added to Queue Storage. 
WebJob listens the queue. When new message is addes WebJob should replace image in Blob Storage with new one with Bloor effect. 
Use streams.
Logs should be written to the Table Storage.

Web Jobs: https://docs.microsoft.com/en-us/azure/app-service/webjobs-create  
Logs to Table Storage: https://www.nuget.org/packages/AzureTableStorageNLogTarget/
