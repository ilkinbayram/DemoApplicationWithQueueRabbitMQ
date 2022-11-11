using System.Threading.Tasks;

namespace Service.Abstract.External
{
    public interface ISocialBotMessagingService
    {
        Task SendMessageAsync(string message);
        Task SendMessageAsync(string newBotToken, long newChatId, string message);
        Task SendPhotoWithMessageAsync(string message, Stream stream, string fileName);
    }
}
