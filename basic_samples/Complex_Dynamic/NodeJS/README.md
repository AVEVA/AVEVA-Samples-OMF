# Complex Dynamic OMF NodeJS Sample

**Version:** 1.0.5

| OCS Test Status                                                                                                                                                                                         | EDS Test Status                                                                                                                                                                                         | PI Test Status                                                                                                                                                                                             |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_DC_nodeJS?jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build?definitionId=1507) | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_DC_nodeJS?jobName=Tests_EDS)](https://dev.azure.com/osieng/engineering/_build?definitionId=1507) | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_DC_nodeJS?jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build?definitionId=1507) |

---

This sample uses OSIsoft Message Format to send data to OSIsoft Cloud Services, Edge Data Store, or PI Web API.

## Requirements

```json
  "dependencies": {
    "request": "2.88.2",
    "request-promise": "4.2.5"
  },
  "devDependencies": {
    "jasmine-reporters": "2.3.2",
    "jasmine": "3.5.0"
  }
```

## Known Security Issues

This sample has an indirect dependency on lodash 4.17.15 because of request-promise 2.88.2. In this version of lodash there is an outstanding security [issue](https://hackerone.com/reports/670779). Please note that using this sample is potentially unsafe because of this issue. Please review the issue before using this.

## Sample Details

See [ReadMe](../)

### Configuration

The sample is configured using the file [config.placeholder.js](config.placeholder.js). Before editing, rename this file to `config.js`. This repository's `.gitignore` rules should prevent the file from ever being checked in to any fork or branch, to ensure credentials are not compromised.

Configure desired OMF endpoints to receive the data in `config.js`.
The following fields are all of the default expected fields:

```js
module.exports = {
  OCS: true,
  PI: false,
  EDS: false,
  omfURL:
    'https://dat-b.osisoft.com/api/v1/Tenants/{tenantid}/Namespaces/{namespace}/omf',
  id: '{id}',
  password: '{password}',
  omfversion: '1.1',
  compression: '',
  WEB_REQUEST_TIMEOUT_SECONDS: '',
  VERIFY_SSL: '',
};
```

note only 1 of OCS, PI, and EDS can be set at a time.
Capitalized fields have default settings that are used if not entered.

`VERIFY_SSL` may be set to false in order to bypass certificate validation. This is helpful if PI Web API is configured to use a self-signed certificate. This will generate a warning; this should only be done for testing with a self-signed PI Web API certificate as it is insecure.

#### OSIsoft Cloud Services

An OMF ingress client must be configured. For an instructional video on creating an OMF Connection go to our [You Tube Learning Channel](https://www.youtube.com/watch?v=52lAnkGC1IM).

1. Set `OCS` to true. All other endpoints need to be empty or not in the .js
1. `omfURL` should be the full omf URL. For example: `https://dat-b.osisoft.com/api/v1/Tenants/{{tenant}}/Namespaces/{{namespace}}/omf`
1. `id` is your clientID
1. `password` is your clientSecret

#### Edge Data Store

1. Set `EDS` to true. All other endpoints need to be empty or not in the .js
1. `omfURL` should be the full omf URL. For example: `http://localhost:5590/api/v1/tenants/default/namespaces/default/omf/`

#### PI Web API

An OMF endpoint must be properly set up and configured.

1. Set `PI` to true. All other endpoints need to be empty or not in the .js
1. `omfURL` should be the full omf URL. For example: `https://{{webapi_machine}}/piwebapi/omf`
1. `id` is your username
1. `password` is your password

## Running the Sample

1. Clone the GitHub repository
1. Install node.js, installation instructions are available at [node.js](https://nodejs.org/en/).
1. Install dependencies, using the command line:

   ```bash
   npm ci
   ```

1. Open Command Prompt in Windows
1. Go to the folder where js files are located
1. Configure desired OMF endpoints to receive the data in `config.js`
1. Type the following command to run the test file in the local server

   ```bash
   node sample.js
   ```

## Running the test

1. Make a local copy of the git repo
1. Install node.js, installation instructions are available at [node.js](https://nodejs.org/en/).
1. Install dependencies, using the command line:

   ```bash
   npm ci
   ```

1. Open Command Prompt in Windows
1. Go to the folder where js files are located
1. Configure desired OMF endpoints to receive the data in `config.js`
1. Type the following command to run the test file in the local server

   ```bash
   npm test
   ```

---

For the OMF landing page [ReadMe](../../../)  
For the OSIsoft Samples landing page [ReadMe](https://github.com/osisoft/OSI-Samples)
