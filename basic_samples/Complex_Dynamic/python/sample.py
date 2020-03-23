
# ************************************************************************
# Import necessary packages
# ************************************************************************

import configparser
import json
import time
import datetime
import platform
import socket
import gzip
import random
import requests
import traceback
import base64
import sys

import auth as auth
import check as check
import sendOMF as sendOMF
import omfHelper as omfHelper 

endpointPI = False
endpointOCS = False
endpointEDS = False

def getAppConfig():

    app_config = {}
    app_config['destinationPI'] = getConfig('Destination', 'PI')
    app_config['destinationOCS'] = getConfig('Destination', 'OCS')
    app_config['destinationEDS'] = getConfig('Destination', 'EDS')
    app_config['omfURL'] = getConfig('Access', 'omfURL')
    app_config['id'] = getConfig('Credentials', 'id')
    app_config['password'] = getConfig('Credentials', 'password')
    app_config['version'] = getConfig('Configurations', 'omfVersion')
    app_config['compression'] = getConfig('Configurations', 'compression')
    timeout = getConfig('Configurations', 'WEB_REQUEST_TIMEOUT_SECONDS')
    verify = getConfig('Configurations', 'VERIFY_SSL')

    if not timeout:
        timeout = 30
    app_config['timeout'] = timeout

    if verify == "False" or verify == "false"or verify == "FALSE":
        verify = False
    else:
        verify = True
    app_config['verify'] = verify
    
    sendOMF.setConfig(app_config)
    check.setConfig(app_config)
    return app_config

def getConfig(section, field):
    # Reads the config file for the field specified
    config = configparser.ConfigParser()
    config.read('config.ini')
    return config.has_option(section, field) and config.get(section, field) or ""

def main(test=False, entries = []):
    # Main program.  Seperated out so that we can add a test function and call this easily
    app_config = {}
    print('Welcome')
    app_config = getAppConfig()

    sendOMF.sendTypeCreate(omfHelper.getType())
    sendOMF.sendContainerCreate(omfHelper.getContainer())

    while not test or len(entries) > 0:
        ans =None
        # can read entries fromt he command line here
        if len(entries) > 0 :
            ans = entries.pop(0)
        else:
            ans = input('Enter pressure, temperature: n to cancel:') 

        if ans == 'n' :
            break

        split = ans.split(',')
        sendOMF.sendDataCreate(omfHelper.getData(pressure = split[0], temperature = split[1]))
    return app_config
            
if __name__ == "__main__":
    sys.argv.pop()
    main(entries = sys.argv)
    print("done")

