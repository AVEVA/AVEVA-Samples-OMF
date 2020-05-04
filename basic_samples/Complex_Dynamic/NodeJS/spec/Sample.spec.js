var Sample = require("../sample");
describe("Complex_Dynamic NodeJS Sample", function () {
  jasmine.DEFAULT_TIMEOUT_INTERVAL = 60000;

  beforeEach(function () {});

  it("should be able to complete the main method", function (done) {
    sample = Sample.app(["1,2", "n"])
      .then(() => {
        if (global.config.OCS) {
          console.log("need to check data here since it is async");

          var restCall = require("request-promise");
          url =
            global.config.omfURL.split("/omf")[0] +
            "/streams/Tank1Measurements/data/last";
          return restCall({
            url: url,
            method: "Get",
            headers: { Authorization: "bearer " + global.authClient.token },
          });
        } else {
          deleteContainer(sample).then(deleteType(sample));
        }
      })
      .catch(function (err) {
        console.log(err);
        throw err;
      })
      .finally(function () {
        done();
      });
  });
});

deleteContainer = function (sample) {
  console.log("Deleting Container");
  containerObj = Sample.omfContainer();
  if (global.authClient.tokenExpires >= sample.nowSeconds) {
    return function (res) {
      refreshToken(res, authClient);
      return omfClient.deleteContainer(containerObj);
    };
  } else {
    return omfClient.deleteContainer(containerObj);
  }
};

deleteType = function (sample) {
  console.log("Delete Type");
  if (authClient.tokenExpires >= sample.nowSeconds) {
    return function (res) {
      refreshToken(res, authClient);
      return omfClient.deleteType(Sample.omfType);
    };
  } else {
    return omfClient.deleteType(Sample.omfType);
  }
};
const { JUnitXmlReporter } = require("jasmine-reporters");
var junitReporter = new JUnitXmlReporter({
  savePath: "TestResults",
});
jasmine.getEnv().addReporter(junitReporter);
