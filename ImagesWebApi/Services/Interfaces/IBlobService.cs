using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImagesWebApi.Interfaces
{
    public interface IBlobService
    {
        Task UploadBlobToStorageAsync(Stream stream, string blobName);

        Task<byte[]> GetBlobByNameAsync(string blobName);
    }
}
