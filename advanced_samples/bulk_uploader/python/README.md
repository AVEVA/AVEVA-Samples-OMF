# Bulk Uploader Python Sample

**Version:** 1.0.0

This sample uses OSIsoft Message Format to send values, streams and types. This simple sample sends OMF messages that are saved pre-formed as files named type.json, container.json, and data.json. It sends the file in that order.

It does only basic error checking to make sure the message was accepted by the endpoint, which means for OCS there is no built-in checking to ensure the upload worked completely. The primary function of this sample is for easy bulk loading of data for other samples (particularly ML based samples where the amount of data is prohibitive to include in the sample itself).

## Requirements

### OSIsoft Message Format Endpoints

Configure desired OMF endpoints to receive the data in [config.ini](.\config.ini). Only one of PI, EDS, or OCS can be configured at a time.

#### OSIsoft Cloud Services

If sending to OSIsoft Cloud Services, set `OCS` to true. This sample needs an OMF cleint credential created. For details on creating those see [OSISoft Learning Channel(https://www.youtube.com/watch?v=52lAnkGC1IM).

1. `omfURL` is the OMFURL as displayed on the protal
1. `id` should be the ID of a [Client Credentials Client](https://cloud.osisoft.com/clients). This client will need to have an OMF Connection configured to the specified Namespace in order to successfully send data. To configure one, pick "OMF" from the "Type" dropdown in the [Connections](https://cloud.osisoft.com/connections) page.
1. `password` should be the secret from the Client Credentials Client that was specified

#### Edge Data Store

If sending to the local Edge Data Store, set `EDS` to true, and update `EdsPort` if using a non-default port. Sending to a remote Edge Data Store is not supported.

#### PI Web API

If sending to PI Web API, set `PI` to true.

1. `omfURL` should be updated with the machine name or fully qualified domain name of the PI Web API server; if possible choose whatever value matches the certificate of the machine
1. PI Web API should have Basic authentication turned on as one of the allowed authentication methods, see [OSIsoft Live Library](https://livelibrary.osisoft.com/LiveLibrary/web/ui.xql?action=html&resource=publist_home.html&pub_category=PI-Web-API)
1. `id` and `password` should be the domain user/password that will be used to perform Basic authentication against PI Web API
1. `VERIFY_SSL` may be set to false in order to bypass certificate validation when PI Web API is configured to use a self-signed certificate. This will generate a warning; this should only be done for testing with a self-signed PI Web API certificate as it is insecure.

## Running the Sample

From the command line, run

```shell
python program.py
```

## Running the Automated Test

To run the automated test, all three OMF endpoint types must be fully configured.

From the command line, run

```shell
pytest test.py
```

The test sends a single OMF type, container, and data message to each of the configured OMF endpoints. Then, the test checks that a value with a recent timestamp is found in OSIsoft Cloud Services. The Edge Data Store and PI Web API OMF endpoints return an HTTP error response if they fail to process an OMF message, so it is not necessary to perform an explicit check against those endpoints. The containers and types are then deleted.

---

For the OMF landing page [ReadMe](../../../)  
For the OSIsoft Samples landing page [ReadMe](https://github.com/osisoft/OSI-Samples)
