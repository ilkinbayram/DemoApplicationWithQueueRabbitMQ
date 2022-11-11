using Service.Abstract.External;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace Services.Concrete.External
{
    public class TelegramMessagingManager : ISocialBotMessagingService
    {
        private TelegramBotClient telClient = new TelegramBotClient("5646898473:AAEuivkIExqaIRtV9vG9KK20qKSj9NgpRec", new HttpClient());

        private List<ChatId> chatIds = new List<ChatId>()
        {
            new ChatId(1050368957)
        };

        public async Task SendMessageAsync(string message)
        {
            foreach (var chatId in chatIds)
            {
                var result = await telClient.SendTextMessageAsync(chatId, message);
            }
        }

        public async Task SendPhotoWithMessageAsync(string message, Stream stream, string fileName)
        {
            foreach (var chatId in chatIds)
            {
                var result = await telClient.SendTextMessageAsync(chatId, message);
                result = await telClient.SendPhotoAsync(chatId, new InputOnlineFile(stream, fileName));
            }
        }

        public async Task SendMessageAsync(string newBotToken, long newChatId, string message)
        {
            TelegramBotClient specialTelegramClient = new TelegramBotClient(newBotToken, new HttpClient());
            ChatId specialChatId = new ChatId(newChatId);

            var result = await specialTelegramClient.SendTextMessageAsync(specialChatId, message);
        }
    }
}
