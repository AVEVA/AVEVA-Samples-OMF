
import requests
import program as program
import json
import sys
import traceback


app_config = {}


def suppressError(call):
    try:
        call()
    except Exception as e:
        print(f"Encountered Error: {e}")


def checkDeletes():
    global app_config


def checkSends(lastVal):
    global app_config


def checkValueGone(url):
    # Sends the request out to the preconfigured endpoint..

    global app_config

    # Assemble headers
    msg_headers = program.getHeaders()

    # Send the request, and collect the response
    if app_config.PI:
        response = requests.get(
            url,
            headers=msg_headers,
            verify=app_config.VERIFY_SSL,
            timeout=app_config.WEB_REQUEST_TIMEOUT_SECONDS,
            auth=(app_config.Id, app_config.Secret)
        )
    else:
        response = requests.get(
            url,
            headers=msg_headers,
            verify=app_config.VERIFY_SSL,
            timeout=app_config.WEB_REQUEST_TIMEOUT_SECONDS,
        )

    # response code in 200s if the request was successful!
    if response.status_code >= 200 and response.status_code < 300:
        response.close()
        print('Value found.  This is unexpected.  "{0}"'.format(
            response.status_code))
        print()
        opId = response.headers["Operation-Id"]
        status = response.status_code
        reason = response.text
        url = response.url
        error = f"  {status}:{reason}.  URL {url}  OperationId {opId}"
        raise Exception(f"Check message was failed. {error}")
    return response.text


def sendTypeDelete():
    program.send_omf_message_to_endpoint(
        "type", program.getFile("type.json"), "delete")


def sendContainerDelete():
    program.send_omf_message_to_endpoint(
        "container", program.getFile("container.json"), "delete")


def checkData():
    global app_config
    if app_config['destinationOCS']:
        checkLastOCSVal()
    # don't have to check others as they are sync and we get instant feedback on success from the app itself


def checkLastOCSVal():
    global app_config
    msg_headers = {
        "Authorization": "Bearer %s" % program.getToken(),
    }
    url = app_config['omfURL'].split(
        '/omf')[0] + '/streams/Tank1Measurements/data/last'
    response = requests.get(
        url,
        headers=msg_headers,
        verify=app_config['verify']
    )

    # response code in 200s if the request was successful!
    if response.status_code < 200 or response.status_code >= 300:
        print(msg_headers)
        response.close()
        print('Response from was bad.  message: {0} {1} {2}.'.format(
            response.status_code, url, response.text))
        print()
        raise Exception("Get value was unsuccessful, {url}. {status}:{reason}".format(
            url=url, status=response.status_code, reason=response.text))


def test_main(onlyDelete: bool = False):
    global app_config
    # Tests to make sure the sample runs as expected

    try:

        program.main(onlyDelete)
        app_config = program.app_config

        if(not onlyDelete):
            checkData()

    except Exception as ex:
        print(f'Encountered Error: {ex}.')
        print
        traceback.print_exc()
        print
        raise ex

    finally:
        print('Deletes')
        print

        suppressError(lambda: sendContainerDelete())
        suppressError(lambda: sendTypeDelete())


if len(sys.argv) >= 1:
    onlyDelete = sys.argv[1]

if __name__ == "__main__":
    test_main(onlyDelete)
