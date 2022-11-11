using Core.Resources.Model;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract.External;
using Services.Abstract.Cloud;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ISocialBotMessagingService _messagingService;
        public NotificationsController(ISocialBotMessagingService messagingService)
        {
            _messagingService = messagingService;
        }

        [HttpPost("PostImage")]
        public async Task<IActionResult> PostNotificationImageAsync(DataMover content)
        {
            string message;
            if (content == null)
            {
                message = "Response is taken with null value!";
                await _messagingService.SendMessageAsync(message);
                return BadRequest(message);
            }

            if (!content.IsSucceesful)
            {
                message = "File couldn't be edited successfully and couldn't be added to the storage! :(";
                await _messagingService.SendMessageAsync(message);
                return BadRequest(message);
            }

            message = $"File edited successfully and added to the storage. You can find the image from storage with this fileName: \"{content.Name}\"";

            using Stream stream = new MemoryStream(content.StreamContent);
            await _messagingService.SendPhotoWithMessageAsync(message, stream, content.Name);

            return Ok();
        }
    }
}
