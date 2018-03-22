from flask import Flask, request
from flask_restful import Resource, Api
from sqlalchemy import create_engine
from json import dumps, loads
from flask.ext.jsonpify import jsonify

app = Flask(__name__)
api = Api(app)

@app.route('/')
def index():
    return "Hello, World!"

tasks = [
    {
        'id': 1,
        'title': u'Buy groceries',
        'description': u'Milk, Cheese, Pizza, Fruit, Tylenol', 
        'done': False
    },
    {
        'id': 2,
        'title': u'Learn Python',
        'description': u'Need to find a good Python tutorial on the web', 
        'done': False
    }
]

@app.route('/todo/api/v1.0/tasks', methods=['POST'])
def get_tasks():
    #user = request.args.get('user')
    data = request.data
    dataDict = loads(data)
    return "Hi " + user + dataDict["name"]
    #return jsonify({'tasks': tasks})


@app.route('/predict', methods=['POST'])
def predict():
    data = request.data
    print("Data = " + data);
    dataDict = loads(data)
    return jsonify(dataDict)


if __name__ == '__main__':
    app.run(debug=True)
