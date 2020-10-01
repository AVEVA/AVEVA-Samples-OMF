# Building a .NET sample to send OMF to PI or OCS

| OCS Test Status                                                                                                                                                                                                                                               | PI Test Status                                                                                                                                                                                                                                                      |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_DotNet?branchName=master&jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build?definitionId=943&branchName=master&jobName=Tests_OCS) | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_DotNet?branchName=master&jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build?definitionId=943&branchName=master&jobName=Tests_OnPrem) |

Developed against DotNet 2.2.300.

## Building a sample with the rest calls directly

The sample does not makes use of the OSIsoft Cloud Services Client Libraries.

The sample also does not use any libraries for connecting to PI. Generally a library will be easier to use.

This sample also doesn't use any help to build the JSON strings for the OMF messages. This works for simple examples, and for set demos, but if building something more it may be easier to not form the JSON messages by hand.

[OMF documentation](https://omf-docs.osisoft.com/)

## Getting Started

In this example we assume that you have the dotnet core CLI.

To run this example from the commandline run

```shell
dotnet restore
dotnet run
```

to test this program change directories to the test and run

```shell
dotnet restore
dotnet test
```

## Configure constants for connecting and authentication

The sample is configured using the file [appsettings.placeholder.json](appsettings.placeholder.json). Before editing, rename this file to `appsettings.json`. This repository's `.gitignore` rules should prevent the file from ever being checked in to any fork or branch, to ensure credentials are not compromised.

The SDS Service is secured by obtaining tokens from Azure Active Directory. Such clients provide a client application identifier and an associated secret (or key) that are authenticated against the directory. You must replace the placeholders in your `appsettings.json` file with the authentication-related values you received from OSIsoft.

```json
{
  "NamespaceId": "REPLACE_WITH_NAMESPACE_ID",
  "TenantId": "REPLACE_WITH_TENANT_ID",
  "Resource": "https://dat-b.osisoft.com",
  "ClientId": "REPLACE_WITH_CLIENT_IDENTIFIER",
  "ClientKey": "REPLACE_WITH_CLIENT_SECRET"
}
```

The PIServer uses the PI Web API as its OMF accepting endpoint. To configure the sample to work against PI update the `appsettings.json` to have only these parameters and update that parameter values to what is being used.

Note: In this sample the tenantId is used to autodetect if you are going against OCS or PI, so make sure that is removed if going against PI.

```json
{
  "Resource": "REPLACE_WITH_PI_WEB_API_URL",
  "dataservername": "REPLACE_WITH_PI_DATA_ARCHIVE_NAME"

  "username": "REPLACE_WITH_USERNAME",
  "password": "REPLACE_WITH_PASSWORD"
}
```

Note: If your username includes a \ you must escape it properly.

See the general readme for information on setting up your endpoint.

If your client computer does not trust the PI Web API certificate you will see an error like:

```shell
System.Net.WebException: The SSL connection could not be established, see inner exception. The remote certificate is invalid according to the validation procedure. ---> System.Net.Http.HttpRequestException: The SSL connection could not be established, see inner exception. ---> System.Security.Authentication.AuthenticationException: The remote certificate is invalid according to the validation procedure.
```

---

Tested against DotNet 2.2.105.

For the general steps or switch languages see the Task [ReadMe](../../)  
For the main OMF page on master [ReadMe](https://github.com/osisoft/OSI-Samples-OMF)  
For the main landing page on master [ReadMe](https://github.com/osisoft/OSI-Samples)
