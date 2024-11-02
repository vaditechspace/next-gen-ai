using AISearchService;
using Azure;
using Azure.AI.OpenAI;
using Azure.Core;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using OpenAI;
using OpenAI.Embeddings;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AISearchService
{
    public class VectorAISearch
    {

        #region Private Variables
        OpenAISearchRequest _request;
        SearchClient _searchClient;
        #endregion

        #region Constructor
        public VectorAISearch(OpenAISearchRequest request)
        {
            _request = request;
            _searchClient = new SearchClient(new Uri(_request.AzAISearchURI), _request.AzAISearchIndex, new AzureKeyCredential(_request.AzAISearchKey));
        }
        #endregion

        #region Public Methods
        /// <summary> 
        /// Performs a vector similarity search based on the given query and repository. 
        /// </summary> 
        /// <param name="query">The search query string.</param> 
        /// <param name="repository">The repository to search within.</param> 
        /// <param name="filename">Optional: The filename to filter the search.</param> 
        /// <returns>A task that represents the asynchronous operation. The task result contains the search results as a string.</returns>
        public async Task<string> VectorAISearchAsync(string query, string repository, string filename = "")
        {
            ReadOnlyMemory<float> queryVector = await GetVectorEmbeddings(query);
            string filter = GetCorpusAISearchFilter(repository, filename);
            // Perform the vector similarity search  
            var searchOptions = new SearchOptions
            {
                //Filter = null,
                Size = 1,
                Select = { "title", "chunk_id", "chunk", "filename", "repository" },
                IncludeTotalCount = true,
                Filter = filter
            };

            searchOptions.VectorSearch = new()
            {
                Queries = {
                        new VectorizedQuery(queryVector)
                        {
                            KNearestNeighborsCount = 3,
                            Fields = { "text_vector" },
                            Exhaustive = true
                        }
                    },
            };

            var vectorSearchResults = _searchClient.Search<SearchDocument>(searchOptions);
            IEnumerable<SearchResult<SearchDocument>> results = vectorSearchResults.Value.GetResults();
            StringBuilder sbResults = new StringBuilder();
            foreach (SearchResult<SearchDocument> result in results)
            {
                if (result.Score < 0.8)
                    continue;

                sbResults.AppendLine(result.Document["chunk"].ToString());
            }
            //Return here
            return sbResults.Length > 0 ? sbResults.ToString() : string.Empty;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// vectorizes the data and returns the vector embeddings. 
        /// </summary>
        /// <param name="input">The text to vectorize</param>
        /// <returns>Task result conetains the vector embeddings as a float array.</returns>
        private async Task<ReadOnlyMemory<float>> GetVectorEmbeddings(string input)
        {
            ReadOnlyMemory<float> vectorInput = null;
            try
            {
                // Use AzureOpenAIClient instead of OpenAIClient
                var azureOpenAiClient = new AzureOpenAIClient(new Uri(_request.AzOpenAIEmbedUri), new ApiKeyCredential(_request.AzOpenAIEmbedApiKey));
                EmbeddingClient eClient = azureOpenAiClient.GetEmbeddingClient(_request.AzOpenAIEmbedModel);
                var embeddingResponse = eClient.GenerateEmbedding(input);
                vectorInput = embeddingResponse.Value.ToFloats();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return vectorInput;
        }
        
        /// <summary>
        /// Set the AI Search filter here
        /// </summary>
        /// <param name="repository">Respository to filter on.</param>
        /// <param name="filename">Filename to filter on</param>
        /// <returns>AI Search filter as string</returns>
        private static string GetCorpusAISearchFilter(string repository, string filename)
        {

            string filter = string.Empty;

            if (!string.IsNullOrEmpty(repository))
            {
                if (!string.IsNullOrEmpty(filter))
                    filter = $"repository eq '{repository}'";
            }

            if (!string.IsNullOrEmpty(filename))
            {
                if (!string.IsNullOrEmpty(filter))
                    filter += $" and ";

                filter += $"filename eq '{filename}'";
            }

            return filter;
        }
       #endregion

    }
}