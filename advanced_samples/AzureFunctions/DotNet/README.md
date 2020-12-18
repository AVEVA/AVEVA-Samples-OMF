# Azure Functions OpenWeather Sample

**Version:** 1.0.7

[![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/AzureFunctions_DotNet?branchName=master)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=1743&branchName=master)

This sample uses Azure Functions and OSIsoft Message Format to send real time data from the [OpenWeather API](https://openweathermap.org/api) to OSIsoft Cloud Services. Once the Azure Function is published, every five minutes it collects current weather data for a specified list of cities, converts that data into OMF, and sends it to OCS.

## Requirements

The [.NET Core CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/) is referenced in this sample, and should be installed to run the sample from the command line.

In order to run this sample as an Azure Function App, a subscription to Microsoft Azure is required. Note that the published Azure Function App may add to Azure subscription costs, so it should be stopped or deleted after use.

[Sign up](https://home.openweathermap.org/users/sign_up) for an OpenWeather account, and get your free API key from the [API keys](https://home.openweathermap.org/api_keys) tab of your account. Alternately, if an API key is not provided, the app will generate random, fake data for the passed list of cities.

### Running the Sample in an Azure Function App

1. Open [OpenWeather.sln](OpenWeather.sln) in Microsoft Visual Studio
1. Right click the OpenWeather project, and publish the function to your Azure subscription, see [Microsoft docs](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#publish-to-azure) for detailed instructions
1. Set up the Configuration of the Azure Function App, this can either be done via the "Edit Azure App Services settings" button in the Publish screen after the function is published, or using the "Configuration" panel of the App Service inside the Azure Portal. The following settings must be added (these are listed in [Program.cs](OpenWeather/Program.cs) in the `LoadConfiguration` function):
   1. `OPEN_WEATHER_URI`, should be `https://api.openweathermap.org/data/2.5/weather` unless changed by OpenWeather
   1. `OPEN_WEATHER_KEY`, should be the API key from your OpenWeather account
   1. `OPEN_WEATHER_QUERIES`, should be a pipe-separated list of cities in the OpenWeather query format, like `San Leandro,us|Philadelphia,us|Johnson City,us|Scottsdale,us`
   1. `OCS_URI`, should be the host specified at the beginning of the URL in the [OCS API Console](https://cloud.osisoft.com/apiconsole)
   1. `OCS_TENANT_ID`, should be the ID that comes after `/Tenants/` in the same URL
   1. `OCS_NAMESPACE_ID`, should be the name of the OCS [Namespace](https://cloud.osisoft.com/namespaces) to receive the data
   1. `OCS_CLIENT_ID`, should be the ID of a [Client Credentials Client](https://cloud.osisoft.com/clients). This client will need to have an OMF Connection configured to the specified Namespace in order to successfully send data. To configure one, pick "OMF" from the "Type" dropdown in the [Connections](https://cloud.osisoft.com/connections) page.
   1. `OCS_CLIENT_SECRET`, should be the secret from the Client Credentials client that was specified
1. Consider storing `OPEN_WEATHER_KEY` and `OCS_CLIENT_SECRET` in an Azure Key Vault
   1. Create new secrets for each of these values in the Azure Key Vault
   1. Enable a system-managed identity for your Azure Function App Service
   1. Grant the App Service identity access to read secrets from the Key Vault
   1. Use `@Microsoft.KeyVault(SecretUri={uri})` in the Configuration values to read them from the Key Vault at runtime
1. Use the `Monitor` panel of the Azure Function to verify that the Azure Function App Service is working as expected, this may require configuring Application Insights first

### Running the Sample locally

1. If you have already published your Azure Function App, you can use the same "Application Settings" window from the "Publish" panel to specify "Local" values of the configuration values for your Azure Function, and run it locally using those settings
1. Alternately, it is possible to fill in the values in [appsettings.placeholder.json](OpenWeather/appsettings.placeholder.json) with the values described above, and then rename the file to `appsettings.json`
1. If `appsettings.json` is present and configured, from the command line, simply run:

```shell
dotnet restore
dotnet run
```

Note: The automated test uses the latter strategy

### Running the Automated Test

To run the automated test, [appsettings.placeholder.json](OpenWeather/appsettings.placeholder.json) should be configured and renamed to `appsettings.json`. The automated test checks that a value is present for the city `San Leandro,us`, so that must be one of the cities specified in `OpenWeatherQueries`.

From the command line, run

```shell
dotnet restore
dotnet test
```

The test sends a type, container, and data message to OCS. Then, the test checks that a value with a recent timestamp is found in OSIsoft Cloud Services.

---

For the OMF landing page [ReadMe](../../../)  
For the OSIsoft Samples landing page [ReadMe](https://github.com/osisoft/OSI-Samples)
