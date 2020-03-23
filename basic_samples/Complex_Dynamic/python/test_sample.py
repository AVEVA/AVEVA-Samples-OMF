
import json
import requests

import sample as sample 
import sendOMF as sendOMF
import omfHelper as omfHelper
import auth as auth

config= {}

def suppressError(call):
    try:
        call()
    except Exception as e:
        print(f"Encountered Error: {e}") 

def checkData():
    if config['destinationOCS']:
        checkLastOCSVal()
    #don't have to check others as they are sync and we get instant feedback on success from the app itself
    
def checkLastOCSVal():
    msg_headers = {
            "Authorization": auth.getAuthHeader()
    }
    url = config['omfURL'].split('/omf')[0] +'/streams/Tank1Measurements/data/last'
    response = requests.get(
    url,
    headers=msg_headers,
    verify=config['verify']
    )
    
    # response code in 200s if the request was successful!
    if response.status_code < 200 or response.status_code >= 300:
        print(msg_headers)
        response.close()
        print('Response from was bad.  "{0}" message: {1} {2}.  Message holdings: {3}'.format(
            message_type, response.status_code, response.text, message_omf_json))
        print()
        raise Exception("OMF message was unsuccessful, {message_type}. {status}:{reason}".format(
            message_type=message_type, status=response.status_code, reason=response.text))

def sendTypeDelete(_type):
    sendOMF.sendCall(json.dumps(_type), 'type', 'delete')

def sendContainerDelete(container):
    sendOMF.sendCall(json.dumps(container), 'container', 'delete')

def test_main():
    global config
    # Tests to make sure the sample runs as expected
    
    try:
        config = sample.main(True, ["2,3","n"] )
        checkData()
    
    except Exception as ex:
        print(f"Encountered Error: {ex}.")
        print
        traceback.print_exc()
        print
        success = False
        if test:
            raise ex

    finally:
        print('Deletes')
        print

        suppressError(lambda: sendContainerDelete(omfHelper.getContainer()))
        suppressError(lambda: sendTypeDelete(omfHelper.getType()))      


