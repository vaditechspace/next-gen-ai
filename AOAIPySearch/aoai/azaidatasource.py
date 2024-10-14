import json

class Authentication:
    def __init__(self, key):
        self.type = "api_key"
        self.key = key

class EmbeddingDependency:
    def __init__(self, deployment_name):
        self.type = "deployment_name"
        self.deployment_name = deployment_name

class Parameters:
    def __init__(self, endpoint, indexName, searchKey, embedModel, semanticConfig, filteron, repository):
        self.endpoint = endpoint
        self.index_name = indexName
        self.semantic_configuration = semanticConfig
        #self.query_type = "vector_semantic_hybrid"
        self.query_type = "vector"
        self.fields_mapping = {}
        self.in_scope = True
        self.role_information = "You are an AI assistant that helps people find information."
        self.filter = f"search.in({filteron}, '{repository}')"
        self.strictness = 3
        self.top_n_documents = 1
        self.authentication = Authentication(searchKey)
        self.embedding_dependency = EmbeddingDependency(embedModel) 

class DataSource:
    def __init__(self, endpoint, indexName, searchKey, embedModel, semanticConfig, filteron, repository):
        self.type = "azure_search"
        self.parameters = Parameters(endpoint, indexName, searchKey, embedModel, semanticConfig, filteron,  repository)
      
    def to_dict(self):
        return json.loads(json.dumps(self, default=lambda o: o.__dict__))
