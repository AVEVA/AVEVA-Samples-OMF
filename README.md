# OMF Samples

The OSIsoft Message Format ([OMF](https://pisquare.osisoft.com/community/developers-club/omf)) defines a set of message headers and bodies that can be used to generate messages for ingestion into a compliant back-end system. The PI System and OCS both have a compliant OMF receiving endpoint.

OMF can be used to develop data acquisition applications on platforms and in languages for which there are no supported OSIsoft libraries. Official documentation can be found [here](https://omf-docs.readthedocs.io/en/latest/).

Some tasks and individual language examples have labels as follows:

The official OMF samples are divided in multiple categories depending on the scenario and problem/task, accessible through the following table:

| Task                                               | Description                                                                                                                                                    | Languages                                                                                                                                                                       | OCS Test Status                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               | PI Test Status                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| -------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **<a href="basic_samples/OMF_API/">Basic API</a>** | These samples demonstrate sending some typical OMF messages. The applications are configurable to both PI and OCS. <a href="basic_samples/OMF_API">Details</a> | <a href="basic_samples/OMF_API/CSharp/OMF_API/">.NET</a><br /><a href="basic_samples/OMF_API/Python3/">Python</a><br /><a href="basic_samples/OMF_API/Java/omfapijava">Java</a> | <img width=100px/> [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_DotNet?branchName=master&jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=943&branchName=master&jobName=Tests_OCS) <br /> [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_Python?branchName=master&jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=949&branchName=master&jobName=Tests_OCS) <br /> [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_Java?branchName=master&jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=945&branchName=master&jobName=Tests_OCS) | <img width=100px/> [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_DotNet?branchName=master&jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=943&branchName=master&jobName=Tests_OnPrem) <br /> [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_Python?branchName=master&jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=949&branchName=master&jobName=Tests_OnPrem) <br /> [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_Java?branchName=master&jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=945&branchName=master&jobName=Tests_OnPrem) |

## Configuring OCS or the PI system to accept OMF messages

Sending to OCS:
Configure OMF Connection. This can be done programmatically, but here are the general steps to do it via the OCS portal:

1. Create an OMF application client credential.
1. Setup the OMF Connection to use the client credential and point to a namespace.
1. Use the OMF Connection information in your application.

Sending to PI:

Use the PI Web API OMF endpoint.  
PI Connector Relay is currently not tested for and would require some changes to the code to get to work.

## OMF limitations on OCS and PI

This list is not exhuastive, but rather a few key details to know.

1. PI only accepts DateTime timestamp as the property index
1. PI can only have 1 index
1. OCS only accepts Dynamic OMFType classification
1. OCS does not accept Link type data

## Credentials

A credential config.ini or app.config file is used in the examples unless otherwise noted in the example.

     Note: This is not a secure way to store credentials.  This is to be used at your own risk.

You will need to modify these files locally when you run the samples.

## Feedback

If you have a need for a new sample; if there is a feature or capability that should be demonstrated; if there is an existing sample that should be in your favorite language; please reach out to us and give us feedback at https://feedback.osisoft.com under the OSIsoft GitHub Channel. [Feedback](https://feedback.osisoft.com/forums/922279-osisoft-github).

## Support

If your support question or issue is related to something with an OSIsoft product (an error message, a problem with product configuration, etc...), please open a case with OSIsoft Tech Support through myOSIsoft Customer Portal (https://my.osisoft.com).

If your support question or issue is related to a non-modified sample (or test) or documentation for the sample; please email Samples@osisoft.com.

## License

[OSI Samples](https://github.com/osisoft/OSI-Samples) are licensed under the [Apache 2 license](../LICENSE.md).

For the main samples landing page on master [ReadMe](https://github.com/osisoft/OSI-Samples)
