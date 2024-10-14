# Azure OpenAI Chatbot

This project, **Azure OpenAI Chatbot**, demonstrates how to build an intelligent chatbot using Azure services, specifically Azure OpenAI and Azure Bot Services, to create conversational AI applications. The bot receives messages from users, processes them using the Azure OpenAI service, and responds accordingly.

## Architecture

This solution follows a cloud-native architecture leveraging the following components:

- **Azure OpenAI**: Provides the AI models (Chat Completion) to process and generate responses to user queries.
- **Azure Bot Service**: Manages the interaction between users and the chatbot, serving as a gateway for different communication channels (e.g., Direct Line).
- **App Service**: Hosts the bot logic (AzureOAIChatBot) and connects with Azure OpenAI through the AOAIProcessor library.

## Project Structure

- **AzureOAIChatBot**: A .NET Bot responsible for receiving user messages, processing them with Azure OpenAI, and sending responses back to the user.
- **AOAIProcessor**: A library project containing the logic for connecting to and interacting with Azure OpenAI to perform chat completions.

## Setup and Deployment

To deploy the AzureOAIChatBot and set up the necessary resources, follow these steps:

1. **Create Azure OpenAI Resource**
   - Create an Azure OpenAI resource in your Azure subscription.
   - Ensure that the chat completion model is deployed within the OpenAI resource.
   - After creating the Azure OpenAI resource, copy the API URL, API Key, and Chat Completion Model Deployment name for use in the bot's appsettings.

2. **App Registration**: 
   - Register a new app in Azure (either single-tenant or multi-tenant) 
   - Generate a client secret and copy the secret value.

3. **Create Azure Bot Resource**
   - In the Azure portal, create a new Azure Bot resource.
   - Provide the necessary details, including linking it to your app registration.

4. **Create App Service**
   - Deploy the AzureOAIChatBot project to an Azure App Service.
   - Use Visual Studio or the Azure CLI for deployment.

5. **Azure Bot Configuration Updates** 
    - After deploying the App Service, copy the App Service URL: https://<<App_NAME>>.azurewebsites.net.
    - Append /api/messages to the URL and enter it in the Messaging Endpoint field in Configuration. The final URL should look like this: https://<<App_NAME>>.azurewebsites.net/api/messages.
    
6. **Update App Settings**
   - Ensure you update the `appsettings.json` file with the Azure OpenAI details:
   ```json
   {
     "MicrosoftAppType": "<<SingleTenant or MultiTenant",
     "MicrosoftAppId": "<<Application (client) ID>>",
     "MicrosoftAppPassword": "<<Client Secret Value>>",
     "MicrosoftAppTenantId": "<<Subscription Id>>",
     "AzOpenAIUrl": "<Your OpenAI API URL>",
     "AzOpenAIApiKey": "<Your OpenAI API Key>",
     "AzOpenAIModel": "<Your Model Name>"
   }

7. **Client Integration**
   - **Update Direct Line Secret**: In your client HTML file, update the Direct Line secret key to establish communication with the bot.

## Running the Application

Once all components are deployed and configured, you can start interacting with the chatbot through the Direct Line channel or other channels supported by Azure Bot Service.

To test the chatbot locally or on a web server, you can run the `ChatDirectline.html` file located in the `ChatBotClient` folder. This file is configured to connect to the bot via the Direct Line channel using the secret key from Azure Bot Service.

## References

- [Azure Bot Service Documentation](https://learn.microsoft.com/en-us/azure/bot-service/?view=azure-bot-service-4.0) – Official documentation for creating, deploying, and managing bots on Azure.
- [Azure OpenAI Service Documentation](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/) – Guide to using the Azure OpenAI Service for processing natural language requests.
- [Direct Line API](https://learn.microsoft.com/en-us/azure/bot-service/rest-api/bot-framework-rest-direct-line-3-0-concepts?view=azure-bot-service-4.0) – Information on how the Direct Line channel works and how to integrate it with clients.
- [Deploy .NET Apps to Azure](https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore?tabs=net80&pivots=development-environment-vs) – Quickstart guide for deploying .NET applications to Azure.
- [Test and debug with the Emulator](https://learn.microsoft.com/en-us/azure/bot-service/bot-service-debug-emulator?view=azure-bot-service-4.0&tabs=csharp) – Quickstart guide to Test and debug with the Emulator.


## Notes

- **Security Consideration**: Storing sensitive information such as app registration credentials and API keys in the app's configuration files is not recommended. It's best practice to use Azure Key Vault to securely manage secrets and access them via environment variables or managed identities.
- **Multi-Tenant or Single-Tenant**: When creating the App Registration, consider whether your bot needs to support multiple organizations (multi-tenant) or just one (single-tenant). This setting impacts authentication and access.
- **Direct Line Configuration**: Make sure the Direct Line secret key is updated in the `ChatDirectline.html` file before running the client-side application.
- **Bot Resource Considerations**: While this bot is an example for interacting with Azure OpenAI, you can extend its functionality by adding other cognitive services or APIs as needed.

## Author

Vadi Raju Parande | [LinkedIn Profile](https://www.linkedin.com/in/yourprofile)
