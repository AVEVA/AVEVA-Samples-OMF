var config = require("./config.js");
const readline = require("readline");
var authObj = require("./auth.js");
var omfObj = require("./omfClient.js");
var sample = require("./Sample.js");

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

sample
  .app(["1,2", "n"])
  .then(function(res) {
    let count = 1;
    function checkFlag() {
      if (count > 50) {
        throw new Error(
          "Main app didn't finish quickly enough.  chacnes are it is hung somewhere..."
        );
      }
      if (global.ending == false) {
        setTimeout(checkFlag, 100);
        count++;
      } else {
        checkData(res).then(
          deleteContainer(res).then(deleteType(res).then(checkDone(res)))
        );
      }
    }

    checkFlag();
  })
  .catch(function(err) {
    logError(err);
  });

checkData = function(res) {
  console.log(res);
  if (global.config.OCS) {
    var restCall = require("request-promise");
    console.log("res2");

    url =
      global.config.omfURL.split("/omf")[0] +
      "/streams/Tank1Measurements/data/last";
    return restCall({
      url: url,
      method: "Get",
      headers: { Authorization: "bearer " + global.authClient.token }
    }).then(function(res) {
      console.log(res);
      return new Promise(function(resolve, reject) {
        resolve();
      });
    });
  } else {
    return new Promise(function(resolve, reject) {
      resolve();
    });
  }
};

deleteContainer = function(res) {
  console.log(res);
  console.log("Deleting Container");
  containerObj = sample.omfContainer();
  if (global.authClient.tokenExpires >= sample.nowSeconds) {
    return function(res) {
      refreshToken(res, authClient);
      return omfClient.deleteContainer(containerObj);
    };
  } else {
    return omfClient.deleteContainer(containerObj);
  }
};
done = false;

deleteType = function(res) {
  console.log(res);
  console.log("Delete Type");
  if (authClient.tokenExpires >= sample.nowSeconds) {
    return function(res) {
      refreshToken(res, authClient);
      return omfClient.deleteType(sample.omfType).then(process.exit(0));
    };
  } else {
    return omfClient.deleteType(sample.omfType).then(process.exit(0));
  }
};
