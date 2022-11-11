using Azure.Abstracts.ModelResource;

namespace AzureStorage.Resources.Base
{
    public class BaseAppendResource : IAppendResource
    {
        public string? ValidToken { get; set; }
    }
}
