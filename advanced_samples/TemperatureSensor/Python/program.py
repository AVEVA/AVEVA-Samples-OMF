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
import time
import datetime
import platform
import socket
import gzip
import random
import traceback
import base64
import urllib3
import distutils.util
import xml.etree.ElementTree as ET

# Specify whether to compress OMF message before
# sending it to ingress endpoint
USE_COMPRESSION = False

# OMF Destinations
OcsOmfUrl = None
EdsOmfUrl = None
PiOmfUrl = None

# Set this to True if your cert is trusted by the Python certify.
# Set to False if you do not want to check the certificate (NOT RECOMMENDED)
VERIFY_SSL = True

# Specify the timeout, in seconds, for sending web requests
# (if it takes longer than this to send a message, an error will be thrown)
WEB_REQUEST_TIMEOUT_SECONDS = 30

ERROR_STRING = "Error"
TYPE_ID = "Temperature.Float"
CONTAINER_ID = "Sample.Script.SL6658.Temperature"

# Holder for user name and password combination
userName = ""
password = ""

clientId = ""
clientSecret = ""

# OCS Token information
__expiration = 0
__token = ""

# Holder for the omfEndPoint base if sending to OCS.  Auth and OMF endpoint are built from this.  It is set from the configuration
resourceBase = ""

# Holder for the sensor URL
sensorUrl = ""

# The version of the OMF messages
omfVersion = "1.1"


def sendOmfMessageToEndpoint(message_type, message_omf_json, action='create'):
    """
    The function takes in a data object and a message type, and it sends an HTTPS
    request to the target OMF endpoint (only PI Web API and EDS are supported in
    the current implementation).
    """
    global omfVersion, userName, password, clientId, clientSecret

    # Compress json omf payload, if specified
    compression = 'none'
    if USE_COMPRESSION:
        msg_body = gzip.compress(bytes(json.dumps(message_omf_json), 'utf-8'))
        compression = 'gzip'
    else:
        msg_body = json.dumps(message_omf_json)

    if (OcsOmfUrl):
        msg_headers = getOcsHeaders(compression, message_type, action)
        response = requests.post(
            OcsOmfUrl,
            headers=msg_headers,
            data=msg_body,
            timeout=WEB_REQUEST_TIMEOUT_SECONDS)
        checkResponse(response, msg_headers, message_type, message_omf_json)
    if (EdsOmfUrl):
        msg_headers = getEdsHeaders(compression, message_type, action)
        response = requests.post(
            EdsOmfUrl,
            headers=msg_headers,
            data=msg_body,
            timeout=WEB_REQUEST_TIMEOUT_SECONDS)
        checkResponse(response, msg_headers, message_type, message_omf_json)
    if (PiOmfUrl):
        msg_headers = getPiHeaders(compression, message_type, action)
        response = requests.post(
            PiOmfUrl,
            headers=msg_headers,
            data=msg_body,
            verify=VERIFY_SSL,
            timeout=WEB_REQUEST_TIMEOUT_SECONDS,
            auth=(userName, password))
        checkResponse(response, msg_headers, message_type, message_omf_json)


def checkResponse(response, msg_headers, message_type, message_omf_json):
    # Response code in 200s if the request was successful!
    if response.status_code < 200 or response.status_code >= 300:
        print(msg_headers)
        response.close()
        print('Response from OMF endpoint was bad.  "{0}" message: {1} {2}.  Message holdings: {3}'.format(
            message_type, response.status_code, response.text, message_omf_json))
        print()
        raise Exception("OMF message was unsuccessful, {message_type}. {status}:{reason}".format(
            message_type=message_type, status=response.status_code, reason=response.text))


def getOcsHeaders(compression="", message_type="", action=""):
    # Assemble headers
    token = getToken()
    msg_headers = {
        'messagetype': message_type,
        'action': action,
        'messageformat': 'JSON',
        'omfversion': omfVersion,
        'Authorization': "Bearer %s" % token
    }

    if(USE_COMPRESSION):
        msg_headers["compression"] = compression

    return msg_headers


def getEdsHeaders(compression="", message_type="", action=""):
    # Assemble headers
    msg_headers = {
        'messagetype': message_type,
        'action': action,
        'messageformat': 'JSON',
        'omfversion': omfVersion
    }

    if(USE_COMPRESSION):
        msg_headers["compression"] = compression

    return msg_headers


def getPiHeaders(compression="", message_type="", action=""):
    # Assemble headers
    msg_headers = {
        'messagetype': message_type,
        'action': action,
        'messageformat': 'JSON',
        'omfversion': omfVersion,
        'x-requested-with': 'xmlhttprequest'
    }

    if(USE_COMPRESSION):
        msg_headers["compression"] = compression

    return msg_headers


def getToken():
    # Gets the token for the omf endpoint
    global __expiration, __token, resourceBase, clientId, clientSecret

    if ((__expiration - time.time()) > 5 * 60):
        return __token

    discoveryUrl = requests.get(
        resourceBase + "/identity/.well-known/openid-configuration",
        headers={"Accept": "application/json"})

    if discoveryUrl.status_code < 200 or discoveryUrl.status_code >= 300:
        discoveryUrl.close()
        print("Failed to get access token endpoint from discovery URL: {status}:{reason}".
              format(status=discoveryUrl.status_code, reason=discoveryUrl.text))
        raise ValueError

    tokenEndpoint = json.loads(discoveryUrl.content)["token_endpoint"]

    tokenInformation = requests.post(
        tokenEndpoint,
        data={"client_id": clientId,
              "client_secret": clientSecret,
              "grant_type": "client_credentials"})

    token = json.loads(tokenInformation.content)

    if token is None:
        raise Exception("Failed to retrieve Token")

    __expiration = float(token['expires_in']) + time.time()
    __token = token['access_token']
    return __token


def oneTimeSendCreates():
    action = 'create'
    oneTimeSendType(action)
    oneTimeSendContainer(action)
    oneTimeSendData(action)


def oneTimeSendDeletes():
    print()
    print("Deleting sample data...")
    print()
    action = 'delete'
    try:
        oneTimeSendData(action)
    except Exception as ex:
        print()
        # Ignore errors in deletes to ensure we clean up as much as possible
        print(("Error in deletes: {error}".format(error=ex)))
        print()

    try:
        oneTimeSendContainer(action)
    except Exception as ex:
        print()
        # Ignore errors in deletes to ensure we clean up as much as possible
        print(("Error in deletes: {error}".format(error=ex)))
        print()

    try:
        oneTimeSendType(action)
    except Exception as ex:
        print()
        # Ignore errors in deletes to ensure we clean up as much as possible
        print(("Error in deletes: {error}".format(error=ex)))
        print()


def oneTimeSendType(action):
    # OMF Type messages
    sendOmfMessageToEndpoint("type", [
        {
            "id": "RemoteAssets.RootType",
            "classification": "static",
            "type": "object",
            "description": "Root remote asset type",
            "properties": {
                "index": {
                    "type": "string",
                    "isindex": True
                },
                "name": {
                    "type": "string",
                    "isname": True
                }
            }
        },
        {
            "id": "RemoteAssets.FuelPumpType",
            "classification": "static",
            "type": "object",
            "description": "Remote pump asset type",
            "properties": {
                "index": {
                    "type": "string",
                    "isindex": True
                },
                "name": {
                    "type": "string",
                    "isname": True
                },
                "Desctiption": {
                    "type": "string",
                    "description": "Description of the asset"
                },
                "Location": {
                    "type": "string",
                    "description": "Location of the asset"
                }
            }
        },
        {
            "id": TYPE_ID,
            "name": "Temperature Float Type",
            "classification": "dynamic",
            "type": "object",
            "properties": {
                "Timestamp": {
                    "format": "date-time",
                    "type": "string",
                    "isindex": True
                },
                "Temperature": {
                    "type": "number",
                    "description": "Temperature readings",
                    "uom": "°F"
                }
            }
        }
    ], action)


def oneTimeSendContainer(action):
    # OMF Container message to create a container for our measurement
    sendOmfMessageToEndpoint("container", [
        {
            "id": CONTAINER_ID,
            "name": "Temperature",
            "typeid": TYPE_ID,
            "description": "Container holds temperature measurements"
        }
    ], action)


def oneTimeSendData(action):
    # OMF Data message to create static elements and create links in AF
    sendOmfMessageToEndpoint("data", [
        {
            "typeid": "RemoteAssets.RootType",
            "values": [
                {
                    "index": "RemoteAssets.Pumps.Root",
                    "name": "Remote Fuel Pumps"
                }
            ]
        },
        {
            "typeid": "RemoteAssets.FuelPumpType",
            "values": [
                {
                    "index": "RemoteAssets.Pump.SL6658",
                    "name": "SL6658 Pump",
                    "Desctiption": "Fuel pump asset",
                    "Location": "SLTC, San Leandro, California"
                }
            ]
        },
        {
            "typeid": "__Link",
            "values": [
                {
                    "source": {
                        "typeid": "RemoteAssets.RootType",
                        "index": "RemoteAssets.Pumps.Root"
                    },
                    "target": {
                        "typeid": "RemoteAssets.FuelPumpType",
                        "index": "RemoteAssets.Pump.SL6658"
                    }
                },
                {
                    "source": {
                        "typeid": "RemoteAssets.FuelPumpType",
                        "index": "RemoteAssets.Pump.SL6658"
                    },
                    "target": {
                        "containerid": CONTAINER_ID
                    }
                }
            ]
        }
    ], action)


def createDataValue(value):
    """Creates a JSON packet containing data value for the container"""
    return [
        {
            "containerid": CONTAINER_ID,
            "values": [
                {
                    "Timestamp": getCurrentTime(),
                    "Temperature": value
                }
            ]
        }
    ]


def getRandomValue():
    """Returns random integer value in 200 - 500 range"""
    value = random.randrange(200, 500)
    return str(value)


def getSensorValue(sensor_url):
    """Simple data collection logic"""
    try:
        response = requests.get(sensor_url)

        if (response.status_code == 200):
            decodedResponse = response.content.decode("utf-8")
            xmlRoot = ET.fromstring(decodedResponse)
            temperatureValue = xmlRoot.find('temperature').text
            print("Sensor value: ", temperatureValue)
            return temperatureValue
        else:
            return ERROR_STRING
    except Exception as ex:
        print(("Encountered Error: {error}".format(error=ex)))
        return ERROR_STRING


def getCurrentTime():
    """Returns the current time in UTC format"""
    return datetime.datetime.utcnow().isoformat() + 'Z'


def getConfig(section, field):
    """Reads the config file for the field specified"""
    config = configparser.ConfigParser()
    config.read('config.ini')
    return config.has_option(section, field) and config.get(section, field) or ""


def main(test=False):
    global omfVersion, userName, password, clientId, clientSecret, resourceBase, OcsOmfUrl, EdsOmfUrl, PiOmfUrl, VERIFY_SSL
    try:
        print('------------------------------------------------------------------')
        print(' .d88888b.  888b     d888 8888888888        8888888b. Y88b   d88P ')
        print('d88P" "Y88b 8888b   d8888 888               888   Y88b Y88b d88P  ')
        print('888     888 88888b.d88888 888               888    888  Y88o88P   ')
        print('888     888 888Y88888P888 8888888           888   d88P   Y888P    ')
        print('888     888 888 Y888P 888 888               8888888P"     888     ')
        print('888     888 888  Y8P  888 888               888           888     ')
        print('Y88b. .d88P 888   "   888 888               888           888     ')
        print(' "Y88888P"  888       888 888      88888888 888           888     ')
        print('------------------------------------------------------------------')

        # Sensor configuration
        useRandom = getConfig('Configurations', 'UseRandom')
        sensorUrl = getConfig('Configurations', 'SensorUrl')

        # Scanning configuration
        iterationCount = (int)(
            getConfig('Configurations', 'NumberOfIterations'))
        delayBetweenRequests = (int)(
            getConfig('Configurations', 'DelayBetweenRequests'))

        # Get OMF endpoint
        SendToOcs = (bool)(distutils.util.strtobool(
            getConfig('Configurations', 'SendToOcs')))
        if (SendToOcs):
            resourceBase = getConfig('Configurations', 'OcsUri')
            OcsTenantId = getConfig('Configurations', 'OcsTenantId')
            OcsNamespaceId = getConfig('Configurations', 'OcsNamespaceId')
            clientId = getConfig('Configurations', 'OcsClientId')
            clientSecret = getConfig('Configurations', 'OcsClientSecret')
            OcsOmfUrl = resourceBase + '/api/v1/tenants/' + OcsTenantId + \
                '/namespaces/' + OcsNamespaceId + '/omf'

        SendToEds = (bool)(distutils.util.strtobool(
            getConfig('Configurations', 'SendToEds')))
        if (SendToEds):
            EdsPort = getConfig('Configurations', 'EdsPort')
            EdsOmfUrl = 'http://localhost:' + EdsPort + \
                '/api/v1/tenants/default/namespaces/default/omf'

        SendToPi = (bool)(distutils.util.strtobool(
            getConfig('Configurations', 'SendToPi')))
        if (SendToPi):
            PiWebApiUrl = getConfig('Configurations', 'PiWebApiUrl')
            userName = getConfig('Configurations', 'UserName')
            password = getConfig('Configurations', 'Password')
            VERIFY_SSL = (bool)(distutils.util.strtobool(
                getConfig('Configurations', 'VerifySSL')))
            if not VERIFY_SSL:
                print('You are not verifying the certificate of the PI Web API endpoint. This is insecure and should not be done in production, please properly handle your certificates. ')
            PiOmfUrl = PiWebApiUrl + '/omf'

        oneTimeSendCreates()

        count = 0
        time.sleep(1)
        while count == 0 or ((not test) and count < iterationCount):
            # Use getSensorValue() method when HW sensor is available or getRandomValue() method to
            # generate random value for demonstration purposes.
            if (useRandom):
                measurement = getRandomValue()
            else:
                measurement = getSensorValue(sensorUrl)

            if(measurement == ERROR_STRING):
                print('Unable to get data from the sensor...')
            else:
                value = int(measurement)/10
                print("Sending value: ", value)
                message = createDataValue(value)
                sendOmfMessageToEndpoint('data', message)

            time.sleep(delayBetweenRequests)
            count = count + 1

        if (test):
            oneTimeSendDeletes()

        print('Complete!')
        return True
    except Exception as ex:
        print()
        msg = "Encountered Error: {error}".format(error=ex)
        print(msg)
        print()
        traceback.print_exc()
        print()
        if (test):
            oneTimeSendDeletes()
        assert False, msg
        return False


if __name__ == "__main__":
    main()
