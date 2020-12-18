# BART API OMF Ingress DotNet Sample

**Version:** 1.0.8

[![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/BartIngress_DotNet?branchName=master)](https://dev.azure.com/osieng/engineering/_build?definitionId=1425&branchName=master)

This sample uses OSIsoft Message Format to send real time data from the Bay Area Rapid Transit (BART) API to OSIsoft Cloud Services, Edge Data Store, and/or PI Web API. Once the sample is started, a timer polls the BART API every 10 seconds for the latest real time estimated times of departure, and sends that data to the configured OMF endpoints.

## Requirements

The [.NET Core CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/) is referenced in this sample, and should be installed to run the sample from the command line.

### Bay Area Rapid Transit

1. Review the BART [Developer License Agreement](https://www.bart.gov/schedules/developers/developer-license-agreement) and get an API validation key from the [BART API website](https://www.bart.gov/schedules/developers/api), or use their public API key
1. The sample is configured using the file [appsettings.placeholder.json](BartIngress\appsettings.placeholder.json). Before editing, rename this file to `appsettings.json`. This repository's `.gitignore` rules should prevent the file from ever being checked in to any fork or branch, to ensure credentials are not compromised.
1. Enter the API key into the `BartApiKey` field of `appsettings.json`
1. (Optional) Update the `BartApiOri` and/or `BartApiDest` fields
   1. These fields specify the origin and destination stations to collect route data for
   1. These should either be a station abbreviation or the special value `"ALL"`, to use all origins and/or destinations
   1. The default is to monitor only the San Leandro (`"SANL"`) - Daly City (`"DALY"`) route
   1. A full list of station abbreviations can be found [here](http://api.bart.gov/docs/overview/abbrev.aspx)

### OSIsoft Message Format Endpoints

Configure desired OMF endpoints to receive the data in `appsettings.json`.

#### OSIsoft Cloud Services

If sending to OSIsoft Cloud Services, set `SendToOcs` to true.

1. `OcsUri` can usually be left as default, but should be the host specified at the beginning of the URL in the [OCS API Console](https://cloud.osisoft.com/apiconsole)
1. `OcsTenantId` should be the ID that comes after `/Tenants/` in the same URL
1. `OcsNamespaceId` should be the name of the OCS [Namespace](https://cloud.osisoft.com/namespaces) to receive the data
1. `OcsClientId` should be the ID of a [Client Credentials Client](https://cloud.osisoft.com/clients). This client will need to have an OMF Connection configured to the specified Namespace in order to successfully send data. To configure one, pick "OMF" from the "Type" dropdown in the [Connections](https://cloud.osisoft.com/connections) page.
1. `OcsClientSecret` should be the secret from the Client Credentials Client that was specified

#### Edge Data Store

If sending to the local Edge Data Store, set `SendToEds` to true, and update `EdsPort` if using a non-default port. Sending to a remote Edge Data Store is not supported.

#### PI Web API

If sending to PI Web API, set `SendToPi` to true.

1. `PiWebApiUrl` should be updated with the machine name or fully qualified domain name of the PI Web API server; if possible choose whatever value matches the certificate of the machine
1. PI Web API should have Basic authentication turned on as one of the allowed authentication methods, see [OSIsoft Live Library](https://livelibrary.osisoft.com/LiveLibrary/web/ui.xql?action=html&resource=publist_home.html&pub_category=PI-Web-API)
1. `Username` and `Password` should be the domain user/password that will be used to perform Basic authentication against PI Web API
1. `ValidateEndpointCertificate` may be set to false in order to bypass certificate validation when PI Web API is configured to use a self-signed certificate. This will generate a warning; this should only be done for testing with a self-signed PI Web API certificate as it is insecure.

## Running the Sample

From the command line, run

```shell
dotnet restore
dotnet run
```

If the `appsettings.json` file has been set up, the sample will first send an OMF type message for BART estimated time of departure (ETD) data ([BartStationEtd.cs](./BartIngress/BartStationEtd.cs)) and an OMF container message for the desired routes. Then, every 10 seconds, the sample will collect real time data and send data messages to the configured OMF endpoints, until the sample is stopped.

## Running the Automated Test

To run the automated test, all three OMF endpoint types must be fully configured.

From the command line, run

```shell
dotnet restore
dotnet test
```

The test sends a single OMF type, container, and data message to each of the configured OMF endpoints. Then, the test checks that a value with a recent timestamp is found in OSIsoft Cloud Services. The Edge Data Store and PI Web API OMF endpoints return an HTTP error response if they fail to process an OMF message, so it is not necessary to perform an explicit check against those endpoints.

---

For the OMF landing page [ReadMe](../../../)  
For the OSIsoft Samples landing page [ReadMe](https://github.com/osisoft/OSI-Samples)
