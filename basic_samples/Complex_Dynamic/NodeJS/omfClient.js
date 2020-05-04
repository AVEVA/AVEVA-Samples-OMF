var restCall = require("request-promise");

module.exports = {
  OMFClient: function (omfURL, client, basicId = null, basicPassword = null) {
    if (client) {
      this.tokenExpires = client.tokenExpires;
      this.getToken = client.getToken;
      this.token = client.token;
    }

    // create a type
    this.createType = function (omfType) {
      return restCall({
        url: omfURL,
        method: "POST",
        headers: this.getHeadersType("type"),
        body: JSON.stringify(omfType).toString(),
        rejectUnauthorized: global.config.VERIFY_SSL,
      });
    };

    // create a container
    this.createContainer = function (omfContainer) {
      return restCall({
        url: omfURL,
        method: "POST",
        headers: this.getHeadersType("container"),
        body: JSON.stringify(omfContainer).toString(),
        rejectUnauthorized: global.config.VERIFY_SSL,
      });
    };

    // create data
    this.createData = function (omfContainer) {
      return restCall({
        url: omfURL,
        method: "POST",
        headers: this.getHeadersType("data"),
        body: JSON.stringify(omfContainer).toString(),
        rejectUnauthorized: global.config.VERIFY_SSL,
      });
    };

    this.deleteType = function (omfType) {
      return restCall({
        url: omfURL,
        method: "POST",
        headers: this.getHeadersType("type", "delete"),
        body: JSON.stringify(omfType).toString(),
        rejectUnauthorized: global.config.VERIFY_SSL,
      });
    };

    // create a container
    this.deleteContainer = function (omfContainer) {
      return restCall({
        url: omfURL,
        method: "POST",
        headers: this.getHeadersType("container", "delete"),
        body: JSON.stringify(omfContainer).toString(),
        rejectUnauthorized: global.config.VERIFY_SSL,
      });
    };

    this.getHeadersType = function (message_type, action = "create") {
      if (basicId) {
        return {
          messagetype: message_type,
          omfversion: "1.1",
          action: action,
          messageformat: "json",
          Authorization:
            "Basic " +
            new Buffer(basicId + ":" + basicPassword).toString("base64"),
          "x-requested-with": "xmlhttprequest",
        };
      } else if (client) {
        return {
          Authorization: "bearer " + client.token,
          messagetype: message_type,
          omfversion: "1.1",
          action: action,
          messageformat: "json",
        };
      } else {
        return {
          messagetype: message_type,
          omfversion: "1.1",
          action: action,
          messageformat: "json",
        };
      }
    };
  },
};
