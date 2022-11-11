using Azure.Abstracts;
using Core.Azure.Config;
using Microsoft.Extensions.Options;
using Services.Abstract.Cloud;

namespace Services.Concrete.Cloud
{
    public class BlobService : BlobStorageRepository, IBlobService
    {
        public BlobService(IOptions<AzureConfig> azureConfig) : base(azureConfig)
        {
        }
    }
}
