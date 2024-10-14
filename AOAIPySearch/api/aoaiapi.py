from aoai.aoaiutility import AOAIUtility
from configloader import ConfigLoader

class RepositoryAPI():
    
    def __init__(self):
        #ConfigLoader.load_config()
        self._azOpenAIConfig = ConfigLoader.GetConfig("azopenai")
        self._azSearchAIConfig = ConfigLoader.GetConfig("azaisearch")

    def CallAZOAISearch(self, prompt):
        aoaiUtility = AOAIUtility(self._azOpenAIConfig, self._azSearchAIConfig)
        aoaiUtility.SetSystemPrompt("You are a helpful and concise assistant. Please provide a brief, clear, and accurate response to the query.")
        return aoaiUtility.SearchAOAI(prompt)

    def CallSearchRepository(self, repository, prompt):
        aoaiUtility = AOAIUtility(self._azOpenAIConfig, self._azSearchAIConfig)
        aoaiUtility.SetSystemPrompt("You are a helpful and concise assistant. Please provide a brief, clear, and accurate response to the query.")
        result = aoaiUtility.AZAISearchOAI(repository, prompt)
        return result
