# Welcome

These OMF API based samples are introductory, language-specific examples of sending data via OMF. They are intended as instructional samples only. These samples do not use any OMF libraries to make their OMF calls.

## Sample Pattern

All OMF API samples are console applications that follow the same sequence of events, allowing you to select the language with which you are most comfortable without missing any instructional features. The pattern followed is:

1. Read Configuration from a file (note some configuration is only settable in the code)
1. Get auth information for endpoint
1. Create static types (PI Only)
1. Create dynamic types
1. Create dynamic types with non-time stamp indicies and multi-key indicies (OCS only)
1. Create containers
1. Send static type data (PI Only)
1. Send link information (PI Only)
1. Send data
1. Cleanup topics and containers sent

These steps illustrate common OMF messages to send. Most OMF sending applications will follow the common paradigm of creating a Type, creating a Container, and then sending Data.

The samples are based on OMF v1.1.

The samples are written in a way that the same sample can send to both PI and OCS. This is controlled by either the crendential file passed in or by an override variable in the program.

The examples for PI are tested against the PI Web API OMF accepting endpoint.

## OMF limitations on OCS and PI

This list is not exhuastive, but rather a few key details to know.

1. PI only accepts DateTime timestamp
1. OCS only accepts Dynamic OMFType classification
1. OCS does not accept Link type

## PI System Client Configuration

If this sample is used against the PI Web API endpoint the client computer must trust the certificate of PI Web API.
The samples assume Basic Authentication

---

| Languages                                                                                                                                                                                            | OCS Test Status                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            | PI Test Status                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [.NET](https://github.com/osisoft/sample-omf-basic_api-dotnet)</br>[Python](https://github.com/osisoft/sample-omf-basic_api-python)</br>[Java](https://github.com/osisoft/sample-omf-basic_api-java) | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-dotnet?repoName=osisoft%2Fsample-omf-basic_api-dotnet&branchName=master&jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2634&repoName=osisoft%2Fsample-omf-basic_api-dotnet&branchName=master) </br> [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-python?repoName=osisoft%2Fsample-omf-basic_api-python&branchName=master&jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2637&repoName=osisoft%2Fsample-omf-basic_api-python&branchName=master) </br> [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-java?repoName=osisoft%2Fsample-omf-basic_api-java&branchName=master&jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2636&repoName=osisoft%2Fsample-omf-basic_api-java&branchName=master) | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-dotnet?repoName=osisoft%2Fsample-omf-basic_api-dotnet&branchName=master&jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2634&repoName=osisoft%2Fsample-omf-basic_api-dotnet&branchName=master) </br> [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-python?repoName=osisoft%2Fsample-omf-basic_api-python&branchName=master&jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2637&repoName=osisoft%2Fsample-omf-basic_api-python&branchName=master) </br> [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/osisoft.sample-omf-basic_api-java?repoName=osisoft%2Fsample-omf-basic_api-java&branchName=master&jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2636&repoName=osisoft%2Fsample-omf-basic_api-java&branchName=master) |

---

For the main OMF page on master [ReadMe](https://github.com/osisoft/OSI-Samples-OMF)  
For the main landing page on master [ReadMe](https://github.com/osisoft/OSI-Samples)
