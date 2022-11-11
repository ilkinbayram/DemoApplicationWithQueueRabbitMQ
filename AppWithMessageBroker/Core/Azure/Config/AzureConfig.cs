using Azure.Abstracts.Configs;

namespace Core.Azure.Config
{
    public class AzureConfig : IAzureConfig
    {
        public string AZR_CONNECTION_STRING { get; set; }
    }
}
