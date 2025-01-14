| :loudspeaker: **Notice**: samples have transitioned to being hosted in individual repositories |
| ---------------------------------------------------------------------------------------------- |

# OMF Samples

The Open Message Format ([OMF](https://omf-docs.osisoft.com/)) defines a set of message headers and bodies that can be used to generate messages for ingestion into a compliant back-end system. The PI System and CONNECT data services both have a compliant OMF receiving endpoint.

OMF can be used to develop data acquisition applications on platforms and in languages for which there are no supported OSIsoft libraries. Official documentation can be found [here](https://omf-docs.osisoft.com/). The PI Square OMF developer community can be found [here](https://pisquare.osisoft.com/community/developers-club/omf).

Some tasks and individual language examples have labels as follows:

The official OMF samples are divided in multiple categories depending on the scenario and problem/task, accessible through the following table:

<table align="middle" width="100%">
  <tr>
    <th align="middle" colspan="2">
      <h2>Tasks</h2>
    </th>
  </tr>
  <tr>
    <td align="middle" valign="top" width="50%">
      <h3>
        <a href="docs/OMF_BASIC.md"> Basic API </a>
      </h3>
      These samples demonstrate sending some typical OMF messages. The
      applications are configurable to both PI and CONNECT data services.
      <a href="docs/OMF_BASIC.md"> Details </a>
      <br />
      <br />
      <table align="middle">
        <tr>
          <th align="middle">Language</th>
          <th align="middle">ADH Test Status</th>
          <th align="middle">EDS Test Status</th>
          <th align="middle">PI Test Status</th>
        </tr>
        <tr>
          <td align="middle">
            <a href="https://github.com/osisoft/sample-omf-basic_api-dotnet">
              .NET
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2634&repoName=osisoft%2Fsample-omf-basic_api-dotnet&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-dotnet?repoName=osisoft%2Fsample-omf-basic_api-dotnet&branchName=main&jobName=Tests_ADH"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2634&repoName=osisoft%2Fsample-omf-basic_api-dotnet&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-dotnet?repoName=osisoft%2Fsample-omf-basic_api-dotnet&branchName=main&jobName=Tests_EDS"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2634&repoName=osisoft%2Fsample-omf-basic_api-dotnet&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-dotnet?repoName=osisoft%2Fsample-omf-basic_api-dotnet&branchName=main&jobName=Tests_OnPrem"
                alt="Build Status"
              />
            </a>
          </td>
        </tr>
        <tr>
          <td align="middle">
            <a href="https://github.com/osisoft/sample-omf-basic_api-python">
              Python
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=3728&repoName=osisoft%2Fsample-omf-basic_api-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-python?repoName=osisoft%2Fsample-omf-basic_api-python&branchName=main&jobName=Tests_ADH"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=3728&repoName=osisoft%2Fsample-omf-basic_api-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-python?repoName=osisoft%2Fsample-omf-basic_api-python&branchName=main&jobName=Tests_EDS"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=3728&repoName=osisoft%2Fsample-omf-basic_api-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-python?repoName=osisoft%2Fsample-omf-basic_api-python&branchName=main&jobName=Tests_OnPrem"
                alt="Build Status"
              />
            </a>
          </td>
        </tr>
        <tr>
          <td align="middle">
            <a href="https://github.com/osisoft/sample-omf-basic_api-cpp">
              C++
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=3580&repoName=osisoft%2Fsample-omf-basic_api-cpp&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-cpp?repoName=osisoft%2Fsample-omf-basic_api-cpp&branchName=main&jobName=Tests_ADH"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=3580&repoName=osisoft%2Fsample-omf-basic_api-cpp&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-cpp?repoName=osisoft%2Fsample-omf-basic_api-cpp&branchName=main&jobName=Tests_EDS"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=3580&repoName=osisoft%2Fsample-omf-basic_api-cpp&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-cpp?repoName=osisoft%2Fsample-omf-basic_api-cpp&branchName=main&jobName=Tests_PI"
                alt="Build Status"
              />
            </a>
          </td>
        </tr>
      </table>
    </td>
  </tr>
  <tr>
    <td align="middle" valign="top" width="50%">
      <h3>
        <a href="docs/COMPLEX_DYNAMIC.md"> Complex Dynamic </a>
      </h3>
      This sample demonstrates sending time series data to the CONNECT data services, Edge Data Store, and PI Web API OMF endpoints. It sends 2 values
      at 1 timestamp.
      <a href="docs/COMPLEX_DYNAMIC.md"> Details </a>
      <br />
      <br />
      <table align="middle">
        <tr>
          <th align="middle">Language</th>
          <th align="middle">ADH Test Status</th>
          <th align="middle">EDS Test Status</th>
          <th align="middle">PI Test Status</th>
        </tr>
        <tr>
          <td align="middle">
            <a
              href="https://github.com/osisoft/sample-omf-complex_dynamic-python"
            >
              Python
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2640&repoName=osisoft%2Fsample-omf-complex_dynamic-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/aveva.sample-omf-complex_dynamic-python?repoName=osisoft%2Fsample-omf-complex_dynamic-python&branchName=main&jobName=Tests_ADH"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2640&repoName=osisoft%2Fsample-omf-complex_dynamic-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/aveva.sample-omf-complex_dynamic-python?repoName=osisoft%2Fsample-omf-complex_dynamic-python&branchName=main&jobName=Tests_EDS"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2640&repoName=osisoft%2Fsample-omf-complex_dynamic-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/aveva.sample-omf-complex_dynamic-python?repoName=osisoft%2Fsample-omf-complex_dynamic-python&branchName=main&jobName=Tests_PI"
                alt="Build Status"
              />
            </a>
          </td>
        </tr>
        <tr>
          <td align="middle">
            <a
              href="https://github.com/osisoft/sample-omf-complex_dynamic-nodejs"
            >
              NodeJS
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2639&repoName=osisoft%2Fsample-omf-complex_dynamic-nodejs&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/aveva.sample-omf-complex_dynamic-nodejs?repoName=osisoft%2Fsample-omf-complex_dynamic-nodejs&branchName=main&jobName=Tests_ADH"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2639&repoName=osisoft%2Fsample-omf-complex_dynamic-nodejs&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/aveva.sample-omf-complex_dynamic-nodejs?repoName=osisoft%2Fsample-omf-complex_dynamic-nodejs&branchName=main&jobName=Tests_EDS"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2639&repoName=osisoft%2Fsample-omf-complex_dynamic-nodejs&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/aveva.sample-omf-complex_dynamic-nodejs?repoName=osisoft%2Fsample-omf-complex_dynamic-nodejs&branchName=main&jobName=Tests_OnPrem"
                alt="Build Status"
              />
            </a>
          </td>
        </tr>
      </table>
    </td>
  </tr>
  <tr>
    <td align="middle" valign="top" width="50%">
      <h3>
        <a href="https://github.com/osisoft/sample-omf-bart_ingress-dotnet">
          Bart Ingress
        </a>
      </h3>
      This sample demonstrates sending time series data to the CONNECT data services, Edge Data Store, and PI Web API OMF endpoints.
      <a href="https://github.com/osisoft/sample-omf-bart_ingress-dotnet">
        Details
      </a>
      <br />
      <br />
      <table align="middle">
        <tr>
          <th align="middle">Language</th>
          <th align="middle">ADH Test Status</th>
          <th align="middle">EDS Test Status</th>
          <th align="middle">PI Test Status</th>
        </tr>
        <tr>
          <td align="middle">
            <a href="https://github.com/osisoft/sample-omf-bart_ingress-dotnet">
              .NET
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2633&repoName=osisoft%2Fsample-omf-bart_ingress-dotnet&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-bart_ingress-dotnet?repoName=osisoft%2Fsample-omf-bart_ingress-dotnet&branchName=main"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2633&repoName=osisoft%2Fsample-omf-bart_ingress-dotnet&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-bart_ingress-dotnet?repoName=osisoft%2Fsample-omf-bart_ingress-dotnet&branchName=main"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2633&repoName=osisoft%2Fsample-omf-bart_ingress-dotnet&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-bart_ingress-dotnet?repoName=osisoft%2Fsample-omf-bart_ingress-dotnet&branchName=main"
                alt="Build Status"
              />
            </a>
          </td>
        </tr>
      </table>
    </td>
  </tr>
  <tr>
    <td align="middle" valign="top" width="50%">
      <h3>
        <a href="https://github.com/osisoft/sample-omf-bulk_upload-python">
          Bulk Upload
        </a>
      </h3>
      This sample demonstrates sending pre-made OMF files to the CONNECT data services, Edge Data Store, and PI Web API OMF endpoints.
      <a href="https://github.com/osisoft/sample-omf-bulk_upload-python">
        Details
      </a>
      <br />
      <br />
      <table align="middle">
        <tr>
          <th align="middle">Language</th>
          <th align="middle">ADH Test Status</th>
          <th align="middle">EDS Test Status</th>
          <th align="middle">PI Test Status</th>
        </tr>
        <tr>
          <td align="middle">
            <a href="https://github.com/osisoft/sample-omf-bulk_upload-python">
              Python
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2638&repoName=osisoft%2Fsample-omf-bulk_upload-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/aveva.sample-omf-bulk_upload-python?repoName=osisoft%2Fsample-omf-bulk_upload-python&branchName=main&jobName=Tests_ADH"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2638&repoName=osisoft%2Fsample-omf-bulk_upload-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/aveva.sample-omf-bulk_upload-python?repoName=osisoft%2Fsample-omf-bulk_upload-python&branchName=main&jobName=Tests_EDS"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2638&repoName=osisoft%2Fsample-omf-bulk_upload-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/aveva.sample-omf-bulk_upload-python?repoName=osisoft%2Fsample-omf-bulk_upload-python&branchName=main&jobName=Tests_PI"
                alt="Build Status"
              />
            </a>
          </td>
        </tr>
      </table>
    </td>
  </tr>
  <tr>
    <td align="middle" valign="top" width="50%">
      <h3>
        <a href="https://github.com/osisoft/sample-omf-azure_functions-dotnet">
          Azure Functions
        </a>
      </h3>
      This sample demonstrates sending time series data to CONNECT data services using an Azure Function App Service.
      <a href="https://github.com/osisoft/sample-omf-azure_functions-dotnet">
        Details
      </a>
      <br />
      <br />
      <table align="middle">
        <tr>
          <th align="middle">Language</th>
          <th align="middle">ADH Test Status</th>
        </tr>
        <tr>
          <td align="middle">
            <a
              href="https://github.com/osisoft/sample-omf-azure_functions-dotnet"
            >
              .NET
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2632&repoName=osisoft%2Fsample-omf-azure_functions-dotnet&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-azure_functions-dotnet?repoName=osisoft%2Fsample-omf-azure_functions-dotnet&branchName=main"
                alt="Build Status"
              />
            </a>
          </td>
        </tr>
      </table>
    </td>
  </tr>
  <tr>
    <td align="middle" valign="top" width="50%">
      <h3>
        <a
          href="https://github.com/osisoft/sample-omf-temperature_sensor-python"
        >
          Temperature Sensor
        </a>
      </h3>
      This sample demonstrates sending periodic temperature data to the OSIsoft
      Cloud Services, Edge Data Store, and PI Web API OMF endpoints.
      <a href="https://github.com/osisoft/sample-omf-temperature_sensor-python">
        Details
      </a>
      <br />
      <br />
      <table align="middle">
        <tr>
          <th align="middle">Language</th>
          <th align="middle">ADH Test Status</th>
          <th align="middle">EDS Test Status</th>
          <th align="middle">PI Test Status</th>
        </tr>
        <tr>
          <td align="middle">
            <a
              href="https://github.com/osisoft/sample-omf-temperature_sensor-python"
            >
              Python
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2641&repoName=osisoft%2Fsample-omf-temperature_sensor-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-temperature_sensor-python?repoName=osisoft%2Fsample-omf-temperature_sensor-python&branchName=main&jobName=Tests_ADH"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2641&repoName=osisoft%2Fsample-omf-temperature_sensor-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-temperature_sensor-python?repoName=osisoft%2Fsample-omf-temperature_sensor-python&branchName=main&jobName=Tests_EDS"
                alt="Build Status"
              />
            </a>
          </td>
          <td align="middle">
            <a
              href="https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2641&repoName=osisoft%2Fsample-omf-temperature_sensor-python&branchName=main"
            >
              <img
                src="https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-temperature_sensor-python?repoName=osisoft%2Fsample-omf-temperature_sensor-python&branchName=main&jobName=Tests_PI"
                alt="Build Status"
              />
            </a>
          </td>
        </tr>
      </table>
    </td>
  </tr>
</table>

## Configuring CONNECT data services or the PI system to accept OMF messages

### Sending to CONNECT data services

Configure OMF Connection. This can be done programmatically, but here are the general steps to do it via the CONNECT data services portal:

If you do not already have a Client-Credentials Client set up to use:

1. After logging in to CONNECT data services, open [Clients](https://datahub.connect.aveva.com/clients), or follow the link under the section "Security"
1. Under "Client Type", select "Client-Credentials"
1. Click "Add Client" and either grant the "Account Administrator" role or whatever custom roles are required to create and write data to Streams
1. Click "Continue," then optionally enter a description and/or update the expiration date for the Client Secret
1. On the "Client Successfully Created" screen, be sure to save the Client Secret as it will not be available again (a new secret can be generated if necessary)

Once you have a Client-Credentials Client for use with OMF:

1. After logging in to CONNECT data services, open [Connections](https://datahub.connect.aveva.com/omf-connections), or follow the link under the section "Data Management"
1. Under "Type", select "OMF"
1. Click "Add Connection" and fill in a name for your OMF Connection
1. Click "Next," and choose the Client-Credentials Client you intend your OMF application to use (or the one you just created above)
1. Click "Next," and choose the Namespace(s) you intend to write OMF data to
1. Click "Next," and review the configuration
1. Click "Save," and you can now use this Client-Credentials Client to write OMF data to the specified Namespace(s) in the samples

### Sending to PI

Use the PI Web API OMF endpoint.  
PI Connector Relay is currently not tested for and would require some changes to the code to get to work.

## OMF limitations on CONNECT data services and PI

This list is not exhuastive, but rather a few key details to know.

1. PI only accepts DateTime timestamp as the property index
1. PI can only have 1 index
1. ADH only accepts Dynamic OMFType classification
1. ADH does not accept Link type data

## Credentials

A credential config.ini or app.config file is used in the examples unless otherwise noted in the example.

     Note: This is not a secure way to store credentials.  This is to be used at your own risk.

You will need to modify these files locally when you run the samples.

## About this repo

The [style guide](https://github.com/osisoft/.github/blob/main/STYLE_GUIDE.md) describes the organization of the repo and the code samples provided. The [test guide](https://github.com/osisoft/.github/blob/main/TEST_GUIDE.md) goes into detail about the included automated tests. The [on prem testing](https://github.com/osisoft/.github/blob/main/ON_PREM_TESTING.md) document describes the software installed on our internal build agent.

## Feedback

To request a new sample, if there is a feature or capability you would like demonstrated, or if there is an existing sample you would like in your favorite language, please give us feedback at [https://feedback.aveva.com](https://feedback.aveva.com) under the Developer Samples category. [Feedback](https://datahub.feedback.aveva.com/ideas/search?category=7135134109509567625&query=sample).

## Support

If your support question or issue is related to something with an AVEVA product (an error message, a problem with product configuration, etc...), please open a case with AVEVA Tech Support through myOSIsoft Customer Portal ([https://my.osisoft.com](https://my.osisoft.com)).

If your support question or issue is related to a non-modified sample (or test) or documentation for the sample; please email Samples@osisoft.com.

## Contributions

If you wish to contribute please take a look at the [contribution guide](https://github.com/osisoft/.github/blob/main/CONTRIBUTING.md).

## License

[OSI Samples](https://github.com/osisoft/OSI-Samples) are licensed under the [Apache 2 license](LICENSE).

For the main samples landing page [ReadMe](https://github.com/osisoft/OSI-Samples)
