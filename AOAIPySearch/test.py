from api.aoaiapi import RepositoryAPI
from configloader import ConfigLoader

ConfigLoader.LoadConfig()
obj = RepositoryAPI()
result = obj.CallSearchRepository("Azure", "How to build a RAG")
print(result)

