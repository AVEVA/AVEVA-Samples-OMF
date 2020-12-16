# Temperature Sensor OMF Python Sample

**Version:** 1.0.1

[![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_Temp_Python?branchName=master)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2164&branchName=master)

This sample uses OSIsoft Message Format to send real time data from a temperature sensor (or random sample values) to OSIsoft Cloud Services, Edge Data Store, and/or PI Web API. Once the sample is started, the sample periodically collects (or generates) values for temperature until it reaches a specified number of values. At each interval it then sends that data to each of the configured OMF endpoints.

## OSIsoft Message Format Endpoints

The sample is configured using the file [config.placeholder.ini](config.placeholder.ini). Before editing, rename this file to `config.ini`. This repository's `.gitignore` rules should prevent the file from ever being checked in to any fork or branch, to ensure credentials are not compromised.

Configure desired OMF endpoints to receive the data in `config.ini`. This sample can send to one each of PI, EDS, and OCS endpoints at a time. This script was designed against OMF version 1.1.

### OSIsoft Cloud Services

If sending to OSIsoft Cloud Services, set `SendToOcs` to true.

1. `OcsUri` can usually be left as default, but should be the host specified at the beginning of the URL in the [OCS API Console](https://cloud.osisoft.com/apiconsole)
1. `OcsTenantId` should be the ID that comes after `/Tenants/` in the same URL
1. `OcsNamespaceId` should be the name of the OCS [Namespace](https://cloud.osisoft.com/namespaces) to receive the data
1. `OcsClientId` should be the ID of a [Client Credentials Client](https://cloud.osisoft.com/clients). This client will need to have an OMF Connection configured to the specified Namespace in order to successfully send data. For details, see [Create an OMF Connection](https://www.youtube.com/watch?v=52lAnkGC1IM) from the OSIsoft Learning Channel.
1. `OcsClientSecret` should be the secret from the Client Credentials Client that was specified

### Edge Data Store

If sending to the local Edge Data Store, set `SendToEds` to true, and update `EdsPort` if using a non-default port. Sending to a remote Edge Data Store is not supported.

### PI Web API

If sending to PI Web API, set `SendToPi` to true.

1. `PiWebApiUrl` should be updated with the machine name or fully qualified domain name of the PI Web API server; if possible choose whatever value matches the certificate of the machine
1. PI Web API should have Basic authentication turned on as one of the allowed authentication methods, see [OSIsoft Live Library](https://livelibrary.osisoft.com/LiveLibrary/web/ui.xql?action=html&resource=publist_home.html&pub_category=PI-Web-API)
1. `Username` and `Password` should be the domain user/password that will be used to perform Basic authentication against PI Web API
1. `VerifySSL` may be set to false in order to bypass certificate validation when PI Web API is configured to use a self-signed certificate. This will generate a warning; this should only be done for testing with a self-signed PI Web API certificate as it is insecure

## Running the Sample

1. Clone the GitHub repository
1. Install required modules: `pip install -r requirements.txt`
1. Open the folder with your favorite IDE
1. Rename `config.placeholder.ini` to `config.ini` and enter the required configuration as described above
1. From the command line, run `python program.py`

Note: The type and container ID, name, and description are hard coded in the sample. The type and container ID can be modified using the `TYPE_ID` and `CONTAINER_ID` variables, and other information can be modified by directly changing the OMF strings in the Python code.

## Running the Automated Test

Complete steps 1-5 above. Then:

1. Run `python test_sample.py`

or

1. Install pytest `pip install pytest`
1. Run `pytest test_sample.py`

The test sends a single value message to each configured endpoint instead of repeating with a delay between values. Once the test is complete the containers and types are deleted.

---

For the OMF landing page [ReadMe](../../../)  
For the OSIsoft Samples landing page [ReadMe](https://github.com/osisoft/OSI-Samples)
