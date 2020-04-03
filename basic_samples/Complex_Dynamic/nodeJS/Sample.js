// sample.js

var config = require("./config.js");
const readline = require("readline");
var authObj = require("./auth.js");
var omfObj = require("./omfClient.js");

// retrieve configuration
var OCS = config.OCS;
var EDS = config.EDS;
var PI = config.PI;
var omfURL = config.omfURL;
var id = config.id;
var password = config.password;
var success = true;
var deleteData = true;
var errorCap = {};
var resource = omfURL.split(".com/")[0] + ".com";
var authClient = {};

global.ending = false;
global.config = config;

global.omfClient = {};

var omfType = [
  {
    id: "TankMeasurement",
    type: "object",
    classification: "dynamic",
    properties: {
      Time: { format: "date-time", type: "string", isindex: true },
      Pressure: {
        type: "number",
        name: "Tank Pressure",
        description: "Tank Pressure in Pa"
      },
      Temperature: {
        type: "number",
        name: "Tank Temperature",
        description: "Tank Temperature in K"
      }
    }
  }
];

var omfContainer = function() {
  return [
    {
      id: "Tank1Measurements",
      typeid: "TankMeasurement",
      typeVersion: "1.0.0.0"
    }
  ];
};

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

var refreshToken = function(res, authClient) {
  var obj = JSON.parse(res);
  authClient.token = obj.access_token;
  authClient.tokenExpires = obj.expires_on;
};

var logError = function(err) {
  success = false;
  errorCap = err;

  console.log("Error");
  console.trace();
  console.log(err.message);
  console.log(err.stack);
  console.log(err.options.headers["Operation-Id"]);
  throw err;
};

var nowSeconds = function() {
  return Date.now() / 1000;
};

ran = false;

var app = function(entries) {
  ran = true;
  var omfURL = config.omfURL;
  var authClient = new authObj.AuthClient(resource);
  global.authClient = authClient;
  omfClient = new omfObj.OMFClient(omfURL, authClient);
  var getClientToken;

  if (OCS) {
    //OCS
    getClientToken = authClient
      .getToken(id, password, resource)
      .then(function(res) {
        refreshToken(res, authClient);
      })
      .catch(function(err) {
        throw err;
      });
  } else if (EDS) {
    //EDS
    getClientToken = new Promise(function(resolve, reject) {
      authClient.tokenExpires = 0;
      resolve();
    });
  } else if (PI) {
    //PI
    getClientToken = new Promise(function(resolve, reject) {
      authClient.tokenExpires = 0;
      omfClient = new omfObj.OMFClient(omfURL, null, id, password);
      resolve();
    });
  }

  var createType = getClientToken
    .then(function(res) {
      console.log("Creating Type");
      if (authClient.tokenExpires >= nowSeconds) {
        return function(res) {
          refreshToken(res, authClient);
          return omfClient.createType(omfType);
        };
      } else {
        return omfClient.createType(omfType);
      }
    })
    .catch(function(err) {
      logError(err);
    });

  var createContainer = createType
    .then(function(res) {
      console.log("Creating Container");
      containerObj = omfContainer();
      if (authClient.tokenExpires >= nowSeconds) {
        return function(res) {
          refreshToken(res, authClient);
          return omfClient.createContainer(containerObj);
        };
      } else {
        return omfClient.createContainer(containerObj);
      }
    })
    .catch(function(err) {
      logError(err);
    });

  var sendDataWrapper = createContainer
    .then(function(res) {
      entriesT = entries;
      console.log("Creating Data");
      if (entriesT.length > 0) {
        entriesT.forEach(function(val, index, array) {
          sendData(val);
        });
      }
      createData();
    })
    .catch(function(err) {
      logError(err);
    });

  var createData = function() {
    if (!global.ending) {
      rl.question("Enter pressure, temperature? n to cancel:", answer => {
        sendData(answer);
      });
    }
  };

  var sendData = function(answer) {
    try {
      if (answer == "n") {
        appFinished();
      } else {
        var arr = answer.split(",");
        var currtime = new Date();
        var dataStr = `[{ "containerid": "Tank1Measurements", "values": [{ "Time": "${currtime.toISOString()}", "Pressure": ${
          arr[0]
        }, "Temperature": ${arr[1]} }] }]`;
        var dataObj = JSON.parse(dataStr);
        if (authClient.tokenExpires >= nowSeconds) {
          return function(res) {
            refreshToken(res, authClient);
            return omfClient.createData(dataObj).then(createData());
          };
        } else {
          return omfClient.createData(dataObj).then(createData());
        }
      }
    } catch (err) {
      logError(err);
      appFinished();
    }
  };

  var appFinished = function() {
    console.log(global.ending);
    global.ending = true;
    console.log();
    console.log(global.ending);

    if (!success) {
      throw errorCap;
    }
    console.log("All values sent successfully!");

    if (require.main === module) {
      process.exit();
    } else return 0;
  };

  if (!success) {
    throw errorCap;
  }

  return getClientToken;
};
module.exports = { app, omfClient, omfType, omfContainer, nowSeconds };

process.argv = process.argv.slice(2);
if (require.main === module) {
  app(process.argv);
}
