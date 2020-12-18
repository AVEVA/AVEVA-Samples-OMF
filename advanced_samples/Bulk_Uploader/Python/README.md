# Bulk Uploader Python Sample

**Version:** 1.0.5

---

| OCS Test Status                                                                                                                                                                                                                             | EDS Test Status                                                                                                                                                                                                                             | PI Test Status                                                                                                                                                                                                                                 |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_BU_Python?branchName=master&jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build?definitionId=1679&branchName=master) | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_BU_Python?branchName=master&jobName=Tests_EDS)](https://dev.azure.com/osieng/engineering/_build?definitionId=1679&branchName=master) | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_BU_Python?branchName=master&jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build?definitionId=1679&branchName=master) |

---

This sample uses OSIsoft Message Format to send values, streams and types. This simple sample sends OMF messages that are saved pre-formed as files named type.json, container.json, and data.json. It sends the files in that order.

It does only basic error checking to make sure the message was accepted by the endpoint, which means for OCS there is no built-in checking to ensure the upload worked completely. The primary function of this sample is for easy bulk loading of data for other samples (particularly ML based samples where the amount of data is prohibitive to include in the sample itself).

## OSIsoft Message Format Endpoints

The sample is configured using the file [config.placeholder.ini](config.placeholder.ini). Before editing, rename this file to `config.ini`. This repository's `.gitignore` rules should prevent the file from ever being checked in to any fork or branch, to ensure credentials are not compromised.

Configure desired OMF endpoints to receive the data in `config.ini`. Only one of PI, EDS, or OCS can be configured at a time. This script was designed against OMF version 1.1.

### OSIsoft Cloud Services

If sending to OSIsoft Cloud Services, set `OCS` to true. This sample needs an OMF client credential created. For details on creating those see [OSIsoft Learning Channel](https://www.youtube.com/watch?v=52lAnkGC1IM).

1. `omfURL` is the OMF URL as displayed on the portal
1. `id` should be the ID of a [Client Credentials Client](https://cloud.osisoft.com/clients). This client will need to have an OMF Connection configured to the specified Namespace in order to successfully send data. To configure one, pick "OMF" from the "Type" dropdown in the [Connections](https://cloud.osisoft.com/connections) page
1. `password` should be the secret from the Client Credentials Client that was specified

### Edge Data Store

If sending to the local Edge Data Store, set `EDS` to true, and update `EdsPort` if using a non-default port. Sending to a remote Edge Data Store is not supported.

### PI Web API

If sending to PI Web API, set `PI` to true.

1. `omfURL` should be updated with the machine name or fully qualified domain name of the PI Web API server; if possible choose whatever value matches the certificate of the machine
1. PI Web API should have Basic authentication turned on as one of the allowed authentication methods, see [OSIsoft Live Library](https://livelibrary.osisoft.com/LiveLibrary/web/ui.xql?action=html&resource=publist_home.html&pub_category=PI-Web-API)
1. `id` and `password` should be the domain user/password that will be used to perform Basic authentication against PI Web API
1. `VERIFY_SSL` may be set to false in order to bypass certificate validation when PI Web API is configured to use a self-signed certificate. This will generate a warning; this should only be done for testing with a self-signed PI Web API certificate as it is insecure

## Running the Sample

1. Clone the GitHub repository
1. Install required modules: `pip install -r requirements.txt`
1. Open the folder with your favorite IDE
1. Update `config.ini` with your credentials
1. Check and update the program to ensure you are sending to OCS or PI.
1. Run `program.py` from commandline run `python program.py`

## Running the Automated Test

Complete steps 1-5 above. Then:

1. Run `python test_sample.py`

or

1. Install pytest `pip install pytest`
1. Run `pytest test_sample.py`

The test sends a single OMF type, container, and data message to each of the configured OMF endpoints. Then, the test checks that a value with a recent timestamp is found in OSIsoft Cloud Services. The Edge Data Store and PI Web API OMF endpoints return an HTTP error response if they fail to process an OMF message, so it is not necessary to perform an explicit check against those endpoints. The containers and types are then deleted.

---

For the OMF landing page [ReadMe](../../../)  
For the OSIsoft Samples landing page [ReadMe](https://github.com/osisoft/OSI-Samples)
