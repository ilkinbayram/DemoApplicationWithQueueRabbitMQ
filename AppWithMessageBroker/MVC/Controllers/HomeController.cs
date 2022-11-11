using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Queue.Data.Abstract;
using Queue.Service.Abstract;
using Services.Abstract.Cloud;
using Shared.Models.BackgroundService;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRMQConnectionService _connectionService;
        private readonly IPublisherService _publisher;
        private readonly IBlobService _blobService;

        public HomeController(ILogger<HomeController> logger,
                              IRMQConnectionService connectionService,
                              IBlobService blobService,
                              IPublisherService publisher)
        {
            _logger = logger;
            _connectionService = connectionService;
            _blobService = blobService;
            _publisher = publisher;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImageLoader()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImageLoader(IFormFile formFile, string editingContent, string newFileName)
        {
            string fileName = formFile.FileName;
            string token = Guid.NewGuid().ToString().ToLower().Replace("-",string.Empty).Substring(0, 7);
            if (!string.IsNullOrEmpty(newFileName))
            {
                fileName = string.Format("{0}_{1}{2}", newFileName, token, Path.GetExtension(formFile.FileName));
            }

            var model = new FileUploadedResponse
            {
                FileNameWithExtension = fileName,
                ContentText = editingContent
            };

            using Stream stream = formFile.OpenReadStream();

            var uploadResult = await _blobService.UploadAsync(stream, fileName);

            if (uploadResult)
            {
                _publisher.Publish(model, "file-edit-queue", "file-edit-exchange", "file-edit-route");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}