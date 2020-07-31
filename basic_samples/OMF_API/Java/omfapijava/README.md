# Building a Java client to send OMF to PI or OCS

| OCS Test Status                                                                                                                                                                                                                                             | PI Test Status                                                                                                                                                                                                                                                    |
| ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_Java?branchName=master&jobName=Tests_OCS)](https://dev.azure.com/osieng/engineering/_build?definitionId=945&branchName=master&jobName=Tests_OCS) | [![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/OMF/OMF_API_Java?branchName=master&jobName=Tests_OnPrem)](https://dev.azure.com/osieng/engineering/_build?definitionId=945&branchName=master&jobName=Tests_OnPrem) |

The sample code in this topic demonstrates how to send OMF messages using Java.

Developed against Maven 3.6.1 and Java 1.8.0_181.

## Summary of steps to run the Java demo

Using Eclipse or any IDE:

1. Clone a local copy of the GitHub repository.

2. Install Maven.

3. If you are using Eclipse, select `File` > `Import` >
   `Maven`> `Existing maven project` and then select the local
   copy.

4. Rename the placeholder config file [config.placeholder.properties](config.placeholder.properties) to `config.properties`

5. Replace the configuration strings in `config.properties`

Using a command line:

1. Clone a local copy of the GitHub repository.

2. Download apache-maven-x.x.x.zip from [maven.apache.org](https://maven.apache.org) and extract it.

3. Setting environment variables.
   a) For Java JDK
   Variable name - JAVA_HOME
   Variable value - location to the Java JDK in User variables.

   and, also add JDK\bin path to the Path variable in System variables.

   b) For Maven
   Variable name - MAVEN_HOME
   Variable value - location to the extracted folder for the
   maven ~\apache-maven-x.x.x in User variables.

   and, also add ~\apache-maven-x.x.x\bin path to the Path variable in System variables.

4. Building and running the project.
   a) cd to your project location.
   b) run `mvn package exec:java` on cmd.

To test your porject locally run `mvn test`

These are also tested using VS Code.

## Configure constants for connecting and authentication

The sample is configured using the file [config.placeholder.properties](config.placeholder.properties). Before editing, rename this file to `config.properties`. This repository's `.gitignore` rules should prevent the file from ever being checked in to any fork or branch, to ensure credentials are not compromised.

The SDS Service is secured by obtaining tokens from Azure Active Directory. Such clients provide a client application identifier and an associated secret (or key) that are authenticated against the directory. You must replace the placeholders your `config.properties` file with the authentication-related values you received from OSIsoft.

The values to be replaced are in `config.properties`:

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

To configure the sample to work against PI update the config.properties to have only these parameters and update that parameter values to what is being used.

Note: the tenantId is used to autodetect if you are going against OCS or PI, so make sure that is removed if going against PI.

```ini
[Configurations]
DataServerName = REPLACE_WITH_PI_DATA_ARCHIVE_NAME

[Access]
Resource = REPLACE_WITH_PI_WEB_API_URL
```

See the general readme for information on setting up your endpoint.

If your client computer Java does not trust the PI Web API certificate you will see an error like:

```shell
javax.net.ssl.SSLHandshakeException: sun.security.validator.ValidatorException: PKIX path building failed: sun.security.provider.certpath.SunCertPathBuilderException: unable to find valid certification path to requested target
```

---

Tested against Maven 3.6.1 and Java 1.8.0_212.

For the general steps or switch languages see the Task [ReadMe](../../)  
For the main OMF page on master [ReadMe](https://github.com/osisoft/OSI-Samples-OMF)  
For the main landing page on master [ReadMe](https://github.com/osisoft/OSI-Samples)
