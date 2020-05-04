# Complex Dynamic OMF Python Sample

| Language                     | OCS Test Status                                                                                                                                                                                                                                    | EDS Test Status                                                                                                                                                                                                                                    | PI Test Status                                                                                                                                                                                                                                        |
| ---------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| <a href="Python/">Python</a> | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_DC_Python?branchName=python&jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=1436&branchName=python) | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_DC_Python?branchName=python&jobName=Tests_EDS)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=1436&branchName=python) | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_DC_Python?branchName=python&jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=1436&branchName=python) |

This sample uses OSIsoft Message Format to send data to OSIsoft Cloud Services, Edge Data Store, or PI Web API.

## Details

This sample sends OMF messages that are Dynamic (meaning the data is indexed) and Complex (meaning more than 1 value per index). The data is a temperature and pressure reading at a timestamp. The sample creates the OMFType, OMFContainer and then waits for keyboard input for the data to send. The input is the temperature and pressure, with time being the time it is received. After the initial one-time creation of the type and container, the application is event-driven. Entries can also be sent as run time parameters

### Test Details

The test checks to make sure that the OMF messages were properly handled by the appropriate OMF endpoint. It leverages that entries can be entered at runtime. It then cleans up endpoints and confirms

---

For the OMF landing page [ReadMe](../../)  
For the OSIsoft Samples landing page [ReadMe](https://github.com/osisoft/OSI-Samples)
