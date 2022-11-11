using AzureStorage.Resources;
using AzureStorage.Resources.Base;

namespace Azure.Abstracts
{
    public interface IBlobStorageRepository
    {
        Task<bool> UploadAsync(Stream stream, string fileName);
        Task<Stream> DownloadAsync(string fileName);
        Task<bool> DeleteAsync(string fileName);

        Task<bool> SetContentAsync<T>(T content, string fileName) where T : BaseAppendResource, new();
        Task<T> GetContentAsync<T>(string fileName, string validToken) where T : BaseAppendResource, new();

        Task<IEnumerable<T>> GetAllContentAsync<T>(string fileName)
            where T : BaseAppendResource, new();

        IEnumerable<string> GetBlobNames(string containerName);
        IEnumerable<string> GetContainerNames();
    }
}
