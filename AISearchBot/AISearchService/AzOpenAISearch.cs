using Azure;
using Azure.Core;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using Azure.AI.OpenAI.Chat;
using System.ClientModel;

namespace AISearchService
{
    public class AzOpenAISearch
    {
        OpenAISearchRequest _request;
        public AzOpenAISearch(OpenAISearchRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// This method has AI Search Extension
        /// </summary>
        /// <param name="query"></param>
        /// <param name="repository"></param>
        /// <param name="username"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<string> TalkToOpenAICognitive(string query, string data)
        {
            AzureOpenAIClient azureClient = new(new Uri(_request.AzOpenAIUri), new ApiKeyCredential(_request.AzOpenAIApiKey));
            ChatClient chatClient = azureClient.GetChatClient(_request.AzOpenAIModel);
            ChatCompletionOptions options = new ChatCompletionOptions
            {
                MaxOutputTokenCount = 150,
                Temperature = (float)0.7,
            };

            ChatCompletion completion = await chatClient.CompleteChatAsync(
                 new List<ChatMessage>()
                {
                    new SystemChatMessage($"Answer the query based on this text: {data}"),
                    new UserChatMessage(query)
                },
                  options
             );

            return completion?.Content?.FirstOrDefault()?.Text ?? "No response generated.";
        }
    }

}


