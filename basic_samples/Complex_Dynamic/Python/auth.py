
import requests
import time
import json


config = {}
__expiration = None
__token = None


def setConfig(_config):
    global config
    config = _config


def getToken():
    # Gets the oken for the omfsendpoint
    global __expiration, __token, config

    if __expiration and ((__expiration - time.time()) > 5 * 60):
        return __token

    # we can't short circuit it, so we must go retreive it.
    baseURL = config['omfURL'].split('.com/')[0] + '.com'

    discoveryUrl = requests.get(
        baseURL + "/identity/.well-known/openid-configuration",
        headers={"Accept": "application/json"},
        verify=config['verify'])

    if discoveryUrl.status_code < 200 or discoveryUrl.status_code >= 300:
        discoveryUrl.close()
        print("Failed to get access token endpoint from discovery URL: {status}:{reason}".
              format(status=discoveryUrl.status_code, reason=discoveryUrl.text))
        raise ValueError

    tokenEndpoint = json.loads(discoveryUrl.content)["token_endpoint"]

    tokenInformation = requests.post(
        tokenEndpoint,
        data={"client_id": config['id'],
              "client_secret": config['password'],
              "grant_type": "client_credentials"},
        verify=config['verify'])

    token = json.loads(tokenInformation.content)

    if token is None:
        raise Exception("Failed to retrieve Token")

    __expiration = float(token['expires_in']) + time.time()
    __token = token['access_token']
    return __token


def getAuthHeader():
    global config

    if config['destinationOCS']:
        return ("Bearer %s" % getToken())
    if config['destinationEDS']:
        return None
    if config['destinationPI']:
        return None
