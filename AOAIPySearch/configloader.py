import json
import os

class ConfigLoader:
    _config = {}

    @classmethod
    def LoadConfig(cls, config_file='config.json'):
        # Load configuration from the specified JSON file
        with open(config_file, 'r') as file:
            cls._config = json.load(file)
    
    @classmethod
    def GetConfig(cls, key):
        # Retrieve a configuration value by key
        return cls._config.get(key)
