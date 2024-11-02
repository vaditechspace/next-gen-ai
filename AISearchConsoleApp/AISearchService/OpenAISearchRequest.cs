using Azure.AI.OpenAI;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISearchService
{
    public class OpenAISearchRequest
    {
        public string AzOpenAIEmbedUri { get; set; } = string.Empty;
        public string AzOpenAIEmbedApiKey { get; set; } = string.Empty;
        public string AzOpenAIEmbedModel { get; set; } = string.Empty;


        public string AzAISearchURI { get; set; } = string.Empty;
        public string AzAISearchKey { get; set; } = string.Empty;
        public string AzAISearchIndex { get; set; } = string.Empty;
    }
}
