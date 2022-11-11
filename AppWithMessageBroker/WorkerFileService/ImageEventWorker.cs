using Core.Resources.Model;
using Core.Resources.Results;
using Operation.Concrete.Base;
using Queue.Service.Abstract;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.Concrete.Operations;
using Services.Abstract.Cloud;
using Shared.Models.BackgroundService;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace WorkerFileService
{
    public class ImageEventWorker : BackgroundService
    {
        private readonly ILogger<ImageEventWorker> _logger;
        private readonly IRMQConnectionService _connectionService;
        private readonly IBlobService _blobService;
        private IModel _channel;

        private const string _fileEditQueueName = "file-edit-queue";

        public ImageEventWorker(ILogger<ImageEventWorker> logger,
                                  IRMQConnectionService connectionService,
                                  IBlobService blobService)
        {
            _logger = logger;
            _connectionService = connectionService;
            _blobService = blobService;
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _connectionService.CreateChannel(_fileEditQueueName);
            _channel.BasicQos(0, 1, true);
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            _channel.BasicConsume(_fileEditQueueName, false, consumer);

            _logger.Log(LogLevel.Information, "Program Is Started...");

            consumer.Received += Consumer_Received;
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            _logger.Log(LogLevel.Information, "Event is cought...");
            Console.WriteLine("Event is cought...");
            await Task.Delay(10000);
            var response = JsonSerializer.Deserialize<FileUploadedResponse>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            var content = await _blobService.DownloadAsync(response.FileNameWithExtension);

            using Image img = Image.FromStream(content);
            using Graphics grph = Graphics.FromImage(img);
            using Font font = new(FontFamily.GenericMonospace, 65, FontStyle.Bold, GraphicsUnit.Pixel);
            var textSize = grph.MeasureString(response.ContentText, font);
            var color = Color.FromArgb(0, 255, 120);
            using SolidBrush brush = new(color);

            var position = new Point(img.Width - ((int)textSize.Width + 40), img.Height - ((int)textSize.Height + 40));

            grph.DrawString(response.ContentText, font, brush, position);

            using MemoryStream stream = new();
            img.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;

            var fileName = $"drawen_{DateTime.Now.ToString("yyyyMMddHHmmss")}_{response.FileNameWithExtension}";

            var dataMover = new DataMover();
            var postClientOperation = new Operation<HttpPostFunction<Response>, Response>();

            var imageByteArr = stream.ToArray();

            dataMover.StreamContent = imageByteArr;
            dataMover.Name = fileName;
            dataMover.IsSucceesful = true;

            var contentToPost = JsonSerializer.Serialize(dataMover);

            var operationResult = await postClientOperation.ExecuteAsync("https://localhost:7071/api/Notifications/PostImage", dataMover);

            if (!operationResult.IsSuccess)
            {
                _logger.LogError($"At the '{this.GetType().Name}' class, '{MethodBase.GetCurrentMethod().Name}' method couldn't completed successfully");
                return;
            }

            await _blobService.DeleteAsync(response.FileNameWithExtension);
            _channel.BasicAck(@event.DeliveryTag, false);
        }
    }
}
