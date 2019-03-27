using System;
using System.IO;
using System.Threading.Tasks;
using ImagesWebApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ImagesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly IQueueService _queueService;

        private readonly ILogger<ImagesController> _logger;

        public ImagesController(IBlobService blobService, IQueueService queueService, ILogger<ImagesController> logger)
        {
            _blobService = blobService;
            _queueService = queueService;

            _logger = logger;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation($"Start getting image with id {id}.");
            try
            {
                var byteArray = await _blobService.GetBlobByNameAsync(id);

                if (byteArray == null)
                {
                    _logger.LogInformation($"No images with id {id} were found.");
                    return NotFound();
                }

                _logger.LogInformation($"Retrieving an immage with id {id}.");


                var stream = new MemoryStream(byteArray);
                return File(stream, "image/" + id.Substring(id.LastIndexOf('.')));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Some error occured while getting an immage with id {id}.");
                throw;
            }
        }

        // POST api/values
        [HttpPost]
        public async Task Post(IFormFile file)
        {
            _logger.LogInformation($"Start uploading image with id {file.FileName} to the server.");
            try
            {
                var stream = file.OpenReadStream();

                var fileName = file.FileName;

                await _blobService.UploadBlobToStorageAsync(stream, fileName);

                await _queueService.WriteMessageToQueueAsync(fileName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Some error occured while uploading an immage with id {file.FileName}.");
                throw;
            }
        }
    }
}
