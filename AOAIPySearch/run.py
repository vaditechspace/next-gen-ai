import json
from api.aoaiapi import RepositoryAPI
from flask import Flask, jsonify, request
from waitress import serve
from configloader import ConfigLoader

# Initialize Flask application
app = Flask(__name__)

# Load configuration settings
ConfigLoader.LoadConfig()

# Define endpoint for AZOAISearch
@app.route('/api/AZOAISearch', methods=['POST'])
def CallAZOAISearch():
    # Get JSON data from request
    data = request.get_json()
    prompt = data['prompt']
    # Validate prompt
    if prompt is None or len(prompt) == 0:
        return jsonify("Please enter a valid prompt")
    # Create instance of RepositoryAPI and call AZOAISearch method
    obj = RepositoryAPI()
    result = obj.CallAZOAISearch(prompt)
    return jsonify(result), 200

# Define endpoint for SearchRepository
@app.route('/api/SearchRepository', methods=['POST'])
def CallSearchRepository():
    # Get JSON data from request
    jsondata = request.get_json()
    repository = jsondata.get('repository')
    prompt = jsondata.get('prompt')
    # Validate prompt
    if prompt is None or len(prompt) == 0:
        return "Please enter a valid prompt", 400
    # Create instance of RepositoryAPI and call SearchRepository method
    obj = RepositoryAPI()
    result = obj.CallSearchRepository(repository, prompt)
    return jsonify(result), 200

# Run the Flask application with debug mode enabled
if __name__ == '__main__':
    app.run(debug=True)
