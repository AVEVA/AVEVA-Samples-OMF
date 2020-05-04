This simple sample sends omf messages that are saved pre-formed as type.json, container.json, and data.json.

It does only basic error checking to make sure the message was accepted by the endpoint, which means for OCS there is no built-in checking to ensure the upload worked completely. The primary function of this sample is for easy bulk loading of data for other samples (particularly ML based samples where the amount of data is prohibitive to include in the sample itself).
