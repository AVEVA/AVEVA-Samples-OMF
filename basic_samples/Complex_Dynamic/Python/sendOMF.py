import requests
import json
import auth as auth


compression = False
verify_ssl = True
config = {}


def setConfig(_config):
    global config

    config = _config
    auth.setConfig(_config)


def sendContainerCreate(container):
    sendCall(json.dumps(container), 'container', 'create')


def sendTypeCreate(_type):
    sendCall(json.dumps(_type), 'type', 'create')


def sendDataCreate(data):
    sendCall(json.dumps(data), 'data', 'create')


def sendCall(msg_body, message_type, action):
    global config

    headers = getHeaders(message_type, action)
    auth = None
    if config['destinationPI']:
        auth = (config['id'], config['password'])

    response = requests.post(
        config['omfURL'],
        headers=headers,
        data=msg_body,
        verify=config['verify'],
        timeout=config['timeout'],
        auth=auth
    )

    # response code in 200s if the request was successful!
    if response.status_code < 200 or response.status_code >= 300:
        print(headers)
        response.close()
        print('Response from was bad.  "{0}" message: {1} {2}.  Message holdings: {3}'.format(
            message_type, response.status_code, response.text, msg_body))
        print()
        raise Exception("OMF message was unsuccessful, {message_type}. {status}:{reason}".format(
            message_type=message_type, status=response.status_code, reason=response.text))


def getHeaders(message_type, action):
    global config

    msg_headers = {
        "x-requested-with": "xmlhttprequest",
        'messagetype': message_type,
        'action': action,
        'messageformat': 'JSON',
        'omfversion': config['version']
    }
    if config['compression'] == "gzip":
        msg_headers["compression"] = "gzip"

    authorization = auth.getAuthHeader()
    if authorization:
        msg_headers["Authorization"] = authorization
    return msg_headers
