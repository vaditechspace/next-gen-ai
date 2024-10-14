import os
import json
from openai import AzureOpenAI
from aoai.azaidatasource import DataSource
from configloader import ConfigLoader

class AOAIUtility:

    #Class Constructor 
    def __init__(self, azOpenAIConfig, azSearchAIConfig, maxTokens=10):
        self._azOpenAIConfig = azOpenAIConfig
        self._azSearchAIConfig = azSearchAIConfig
        self._maxTokens = maxTokens
        ConfigLoader.LoadConfig()
    
    #Azure Open AI (Chat Comletion) method, where you send prompt and get the reply back.
    def SearchAOAI(self, prompt):
        try:
            messagesArray = [
                {
                    "role": "system",
                    "content": self.sysPrompt
                },
                {
                    "role": "user",
                    "content": prompt
                }
            ]
            #Get Azure OpenAI client reference here.             
            aoaiClient = self.GetAOAIClient()
            #Search Azure OpenAI passing the prompt
            response = aoaiClient.chat.completions.create(
                model=self._model, 
                messages=messagesArray, 
                max_tokens=self._maxTokens,
                temperature=0.7,
                top_p=0.95,
                )
            #Get the results here
            result = response.choices[0].message.content
            
            #Prepare JSON response
            json_result = {
                "result": result
            }
        except Exception as e:
            json_result = {"error": f"An error occurred: {e}"}
        
        return json.dumps(json_result)  # Convert to JSON string

    def AISearchDataSource(self, repository):
        # Example usage
        searchEndpoint = self._azSearchAIConfig["endpoint"]
        searchKey = self._azSearchAIConfig["key"]
        indexName = self._azSearchAIConfig["index"]
        embedModel = self._azSearchAIConfig["embedmodel"]
        semanticConfig = "" #self._azSearchAIConfig["semanticconfiguration"]
        filteron = self._azSearchAIConfig["filteron"]
        dataSource = DataSource(searchEndpoint, indexName, searchKey, embedModel, semanticConfig, filteron, repository)
        dataSourceDict = dataSource.to_dict()
        return dataSourceDict

    #Azure Open AI Search with Chat Comletion method, where you send prompt and get the reply back.
    def AZAISearchOAI(self, repository, prompt):
        #Get Azure OpenAI client reference here.             
        aoaiClient = self.GetAOAIClient()
        dataSourceDict = self.AISearchDataSource(repository)
        
        response = aoaiClient.chat.completions.create(
                model=self._model,
                messages=self.GetMessages(prompt),
                max_tokens=100,
                temperature=0,
                top_p=1,
                frequency_penalty=0,
                presence_penalty=0,
                stop=None,
                stream=False,
                extra_body= {
                "data_sources": [dataSourceDict]
                }
        )
        
        #Get the results here
        result = response.choices[0].message.content
        
        #Prepare JSON response
        json_result = {
            "result": result
        }

        return json_result

    #Get the Azure OpenAI client reference here. 
    def GetAOAIClient(self):
        client = AzureOpenAI(
        api_key=self._azOpenAIConfig["key"],  
        api_version=self._azOpenAIConfig["apiversion"],
        azure_endpoint = self._azOpenAIConfig["endpoint"])
        self._model =  self._azOpenAIConfig["model"]
        return client
    
    def SetSystemPrompt(self, sysPrompt):
        self.sysPrompt = sysPrompt

    def GetMessages(self, prompt):
        return [
                {
                    "role": "system",
                    "content": self.sysPrompt
                },
                {
                    "role": "user",
                    "content": prompt
                }
            ]
