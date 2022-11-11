using Queue.Data.Abstract;

namespace Shared.Models.BackgroundService
{
    public class FileUploadedResponse : IEventBody
    {
        public string FileNameWithExtension { get; set; }
        public string ContentText { get; set; }
    }
}
