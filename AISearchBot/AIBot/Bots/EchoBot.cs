// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.22.0

using AISearchService;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        private readonly IConfiguration _configuration;
        public EchoBot(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = "";
            try
            {
                OpenAISearchRequest openAISearchRequest = new OpenAISearchRequest();
                PopulateAIRequest(ref openAISearchRequest);

                VectorAISearch vaisearch = new VectorAISearch(openAISearchRequest);
                string result = await vaisearch.VectorAISearchAsync(turnContext.Activity.Text,"");
                
                AzOpenAISearch azOpenAISearch = new AzOpenAISearch(openAISearchRequest);
                replyText = await azOpenAISearch.TalkToOpenAICognitive(turnContext.Activity.Text, result);

                await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
            }
            catch (Exception ex)
            {
                replyText = ex.InnerException.Message;
                await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
            }

        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        private void PopulateAIRequest(ref OpenAISearchRequest openAISearchRequest)
        {
            //Azure OpenAI
            openAISearchRequest.AzOpenAIUri = _configuration["AzOAIUri"]; ;
            openAISearchRequest.AzOpenAIApiKey = _configuration["AzOAIApiKey"];
            openAISearchRequest.AzOpenAIModel = _configuration["AzOAIModel"];
            //openAISearchRequest.AzOAIEmbedModel = _configuration["AzOAIEmbedModel"];

            //Azure OpenAI Embed
            openAISearchRequest.AzOpenAIEmbedUri = _configuration["AzOAIEmbedUri"]; ;
            openAISearchRequest.AzOpenAIEmbedApiKey = _configuration["AzOAIEmbedKey"];
            openAISearchRequest.AzOpenAIEmbedModel = _configuration["AzOAIEmbedModel"];
            

            //Azure AI Search
            openAISearchRequest.AzAISearchURI = _configuration["AISEndPoint"];
            openAISearchRequest.AzAISearchKey = _configuration["AISKey"];
            openAISearchRequest.AzAISearchIndex = _configuration["AzAISearchIndex"];
        }

    }
}
