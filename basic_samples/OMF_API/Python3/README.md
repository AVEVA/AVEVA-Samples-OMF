# Building a Python client to send OMF to PI or OCS

| OCS Test Status                                                                                                                                                                                                                                               | PI Test Status                                                                                                                                                                                                                                                      |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_Python?branchName=master&jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build?definitionId=949&branchName=master&jobName=Tests_OCS) | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_Python?branchName=master&jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build?definitionId=949&branchName=master&jobName=Tests_OnPrem) |

The sample code in this topic demonstrates how to send OMF messages using Python.

The samples were built and tested against Python 3. If you are using a different version you might encounter errors or unexepected behavior.

To Run this Sample:

---

1. Clone the GitHub repository
1. Install required modules: `pip install -r requirements.txt`
1. Open the folder with your favorite IDE
1. Rename the placeholder config file [config.placeholder.ini](config.placeholder.ini) to `config.ini`
1. Update `config.ini` with tour credentials
1. Check and update the program to ensure you are sending to OCS or PI.
1. Run `program.py` from commandline run `python program.py`

To test the sample after running it:

1. Run `python test.py`

or

1. Install pytest `pip install pytest`
1. Run `pytest program.py`

## Configure constants for connecting and authentication

The sample is configured using the file [config.placeholder.ini](config.placeholder.ini). Before editing, rename this file to `config.ini`. This repository's `.gitignore` rules should prevent the file from ever being checked in to any fork or branch, to ensure credentials are not compromised.

The SDS Service is secured by obtaining tokens from Azure Active Directory. Such clients provide a client application identifier and an associated secret (or key) that are authenticated against the directory. You must replace the placeholders in your `config.ini` file with the authentication-related values you received from OSIsoft.

The values to be replaced are in `config.ini`:

```ini
[Configurations]
Namespace = Samples

[Access]
Resource = https://dat-b.osisoft.com
Tenant = REPLACE_WITH_TENANT_ID
ApiVersion = v1

[Credentials]
ProducerToken = REPLACE_WITH_TOKEN_STRING
ClientId = REPLACE_WITH_APPLICATION_IDENTIFIER
ClientSecret = REPLACE_WITH_APPLICATION_SECRET
```

The PIServer will use the PI Web API as its OMF accepting endpoint. This is what the sample is tested against. Currently the only OMF supported endpoint for PI is the Connector Relay. These samples have not been tested against this.

To configure the sample to work against PI update the `config.ini` to have only these parameters and update that parameter values to what is being used.

Note: the tenantId is used to autodetect if you are going against OCS or PI, so make sure that is removed if going against PI.

```ini
[Configurations]
DataServerName = REPLACE_WITH_PI_DATA_ARCHIVE_NAME

[Access]
Resource = REPLACE_WITH_PI_WEB_API_URL
```

See the general readme for information on setting up your endpoint.

If your client computer Python does not trust the PI Web API certificate you will see an error like:

```shell
requests.exceptions.SSLError: HTTPSConnectionPool(host='...', port=443): Max retries exceeded with url: /piwebapi/omf (Caused by SSLError(SSLCertVerificationError(1, '[SSL: CERTIFICATE_VERIFY_FAILED] certificate verify failed: self signed certificate (_ssl.c:1051)')))
```

Complete dependencies listed in [DEPENDENCIES.md](DEPENDENCIES.md)

---

For the general steps or switch languages see the Task [ReadMe](../)  
For the main OMF page on master [ReadMe](https://github.com/osisoft/OSI-Samples-OMF)  
For the main landing page on master [ReadMe](https://github.com/osisoft/OSI-Samples)
