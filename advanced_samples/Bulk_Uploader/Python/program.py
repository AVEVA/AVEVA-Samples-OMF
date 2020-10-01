# NOTE: this script was designed using the v1.1
# version of the OMF specification, as outlined here:
# https://omf-docs.osisoft.com/documentation_v11/Whats_New.html
# *************************************************************************************

# ************************************************************************
# Import necessary packages
# ************************************************************************

import configparser
import json
import requests
import traceback
import time

app_config = {}


def getToken():
    # Gets the oken for the omfsendpoint
    global app_config

    if app_config['destinationEDS']:
        return
    if app_config['destinationPI']:
        return

    if ('__expiration' in app_config and (app_config['__expiration'] - time.time()) > 5 * 60):
        return app_config['__token']

    # we can't short circuit it, so we must go retreive it.

    baseURL = app_config['omfURL'].split('.com/')[0] + '.com'

    discoveryUrl = requests.get(
        baseURL + "/identity/.well-known/openid-configuration",
        headers={"Accept": "application/json"},
        verify=app_config['verify'])

    if discoveryUrl.status_code < 200 or discoveryUrl.status_code >= 300:
        discoveryUrl.close()
        print("Failed to get access token endpoint from discovery URL: {status}:{reason}".
              format(status=discoveryUrl.status_code, reason=discoveryUrl.text))
        raise ValueError

    tokenEndpoint = json.loads(discoveryUrl.content)["token_endpoint"]

    tokenInformation = requests.post(
        tokenEndpoint,
        data={"client_id": app_config['id'],
              "client_secret": app_config['password'],
              "grant_type": "client_credentials"},
        verify=app_config['verify'])

    token = json.loads(tokenInformation.content)

    if token is None:
        raise Exception("Failed to retrieve Token")

    app_config['__expiration'] = float(token['expires_in']) + time.time()
    app_config['__token'] = token['access_token']
    return app_config['__token']


def send_omf_message_to_endpoint(message_type, msg_body, action='create'):
    # Sends the request out to the preconfigured endpoint..

    global app_config
    # Compress json omf payload, if specified
    # msg_body = json.dumps(message_omf_json)

    msg_headers = getHeaders(message_type, action)
    response = {}

    # Assemble headers
    if app_config['destinationPI']:
        response = requests.post(
            app_config['omfURL'],
            headers=msg_headers,
            data=msg_body,
            verify=app_config['verify'],
            timeout=app_config['timeout'],
            auth=(app_config['id'], app_config['password'])
        )
    else:
        response = requests.post(
            app_config['omfURL'],
            headers=msg_headers,
            data=msg_body,
            verify=app_config['verify'],
            timeout=app_config['timeout']
        )

    # response code in 200s if the request was successful!
    if response.status_code < 200 or response.status_code >= 300:
        print(msg_headers)
        response.close()
        print('Response from was bad.  "{0}" message: {1} {2}.  Message holdings: {3}'.format(
            message_type, response.status_code, response.text, msg_body))
        print()
        raise Exception("OMF message was unsuccessful, {message_type}. {status}:{reason}".format(
            message_type=message_type, status=response.status_code, reason=response.text))


def getHeaders(message_type="", action=""):
    global app_config

    # Assemble headers
    if app_config['destinationOCS']:
        msg_headers = {
            "Authorization": "Bearer %s" % getToken(),
            'messagetype': message_type,
            'action': action,
            'messageformat': 'JSON',
            'omfversion': app_config['version']
        }
    elif app_config['destinationEDS']:
        msg_headers = {
            'messagetype': message_type,
            'action': action,
            'messageformat': 'JSON',
            'omfversion': app_config['version']
        }
    else:
        msg_headers = {
            "x-requested-with": "xmlhttprequest",
            'messagetype': message_type,
            'action': action,
            'messageformat': 'JSON',
            'omfversion': app_config['version']
        }
    return msg_headers


def getConfig(section, field):
    # Reads the config file for the field specified
    config = configparser.ConfigParser()
    config.read('config.ini')
    return config.has_option(section, field) and config.get(section, field) or ""


def getAppConfig():
    global app_config
    app_config = {}
    app_config['destinationPI'] = getConfig('Destination', 'PI')
    app_config['destinationOCS'] = getConfig('Destination', 'OCS')
    app_config['destinationEDS'] = getConfig('Destination', 'EDS')
    app_config['omfURL'] = getConfig('Access', 'omfURL')
    app_config['id'] = getConfig('Credentials', 'id')
    app_config['password'] = getConfig('Credentials', 'password')
    app_config['version'] = getConfig('Configuration', 'omfVersion')
    app_config['compression'] = getConfig('Configuration', 'compression')
    timeout = getConfig('Configuration', 'WEB_REQUEST_TIMEOUT_SECONDS')
    verify = getConfig('Configuration', 'VERIFY_SSL')

    if not timeout:
        timeout = 30
    app_config['timeout'] = timeout

    if verify == "False" or verify == "false"or verify == "FALSE":
        verify = False
    else:
        verify = True
    app_config['verify'] = verify

    return app_config


def getFile(file):
    with open(file) as myfile:
        return "".join(line.rstrip() for line in myfile)


def main(onlyConfigure: bool = False):
    # Main program.  Seperated out so that we can add a test function and call this easily
    global app_config
    try:
        print("getting configuration")
        getAppConfig()

        if onlyConfigure:
            return

        print("sending types")
        send_omf_message_to_endpoint("type", getFile("type.json"))

        print("sending containers")
        send_omf_message_to_endpoint("container", getFile("container.json"))

        print("sending data")
        send_omf_message_to_endpoint("data", getFile("data.json"))

    except Exception as ex:
        print(("Encountered Error: {error}".format(error=ex)))
        print
        traceback.print_exc()
        print
        raise ex
    finally:
        print("done")


if __name__ == "__main__":
    main()
