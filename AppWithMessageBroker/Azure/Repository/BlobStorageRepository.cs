using Azure.Abstracts.Configs;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using AzureStorage.Resources;
using AzureStorage.Resources.Base;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Azure.Abstracts
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private readonly BlobServiceClient _serviceClient;
        public BlobStorageRepository(IOptions<IAzureConfig> azureConfig)
        {
            _serviceClient = new BlobServiceClient(azureConfig.Value.AZR_CONNECTION_STRING);
        }

        public async Task<bool> DeleteAsync(string fileName)
        {
            var containerName = CreateContainerName(fileName);
            var containerClient = _serviceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            var result = await blobClient.DeleteAsync();
            return !result.IsError;
        }

        public async Task<Stream> DownloadAsync(string fileName)
        {
            var containerName = CreateContainerName(fileName);

            var containerClient = _serviceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            var info = await blobClient.DownloadContentAsync();
            return info.Value.Content.ToStream();
        }

        public async Task<T> GetContentAsync<T>(string fileName, string validToken)
            where T : BaseAppendResource, new()
        {
            var containerClient = _serviceClient.GetBlobContainerClient(ContainerType.txt.ToString());
            await containerClient.CreateIfNotExistsAsync();
            var appendBlobClient = containerClient.GetAppendBlobClient(fileName + ".txt");
            await appendBlobClient.CreateIfNotExistsAsync();

            var readStream = await appendBlobClient.DownloadAsync();
            List<T> list;

            using (StreamReader reader = new StreamReader(readStream.Value.Content))
            {
                string line = await reader.ReadToEndAsync();
                list = JsonSerializer.Deserialize<List<T>>(string.Format("[{0}]", line));

                if (list == null) return null;

                return list.FirstOrDefault(x => x.ValidToken == validToken);
            }
        }

        public async Task<IEnumerable<T>> GetAllContentAsync<T>(string fileName)
            where T : BaseAppendResource, new()
        {
            var containerClient = _serviceClient.GetBlobContainerClient(ContainerType.txt.ToString());
            await containerClient.CreateIfNotExistsAsync();
            var appendBlobClient = containerClient.GetAppendBlobClient(fileName + ".txt");
            await appendBlobClient.CreateIfNotExistsAsync();

            var readStream = await appendBlobClient.DownloadAsync();

            using (StreamReader reader = new StreamReader(readStream.Value.Content))
            {
                string line = await reader.ReadToEndAsync();

                return JsonSerializer.Deserialize<List<T>>(string.Format("[{0}]", line));
            }
        }

        public IEnumerable<string> GetBlobNames(string containerName)
        {
            var containerClient = _serviceClient.GetBlobContainerClient(containerName);
            var allBlobs = containerClient.GetBlobs();
            return allBlobs.Select(x => x.Name);
        }

        public IEnumerable<string> GetContainerNames()
        {
            var result = _serviceClient.GetBlobContainers();

            return result.Select(x => x.Name);
        }

        public async Task<bool> SetContentAsync<T>(T content, string fileName)
            where T : BaseAppendResource, new()
        {
            var containerClient = _serviceClient.GetBlobContainerClient(ContainerType.txt.ToString());
            await containerClient.CreateIfNotExistsAsync();
            var appendBlobClient = containerClient.GetAppendBlobClient(fileName + ".txt");
            await appendBlobClient.CreateIfNotExistsAsync();
            bool sentSuccessfully;

            var readStream = await appendBlobClient.DownloadAsync();
            List<T> list;

            using (StreamReader reader = new StreamReader(readStream.Value.Content))
            {
                string line = await reader.ReadToEndAsync();
                list = JsonSerializer.Deserialize<List<T>>(string.Format("[{0}]", line));

                if (list == null)
                    list = new List<T>();
            }

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(ms))
                {
                    content.ValidToken = Guid.NewGuid().ToString().ToLower().Replace("-", string.Empty);
                    if (list != null && list.Any())
                        writer.Write(string.Format(",{0}", JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true})));
                    else
                        writer.Write(string.Format("{0}", JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true })));
                    writer.Flush();
                    ms.Position = 0;

                    var appendResult = await appendBlobClient.AppendBlockAsync(ms);
                    sentSuccessfully = !appendResult.GetRawResponse().IsError;
                }
            }

            return sentSuccessfully;
        }

        public async Task<bool> UploadAsync(Stream stream, string fileName)
        {
            var containerName = CreateContainerName(fileName);
            var containerClient = _serviceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            var blobClient = containerClient.GetBlobClient(fileName);
            var response = await blobClient.UploadAsync(stream);
            return !response.GetRawResponse().IsError;
        }


        private string CreateContainerName(string fileName)
        {
            return Path.GetExtension(fileName).Replace(".", string.Empty);
        }
    }
}
