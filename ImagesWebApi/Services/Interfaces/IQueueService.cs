using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagesWebApi.Interfaces
{
    public interface IQueueService
    {
        Task WriteMessageToQueueAsync(string message);
    }
}
