// auth.js
//

var restCall = require('request-promise');

var logError = function(err) {
  success = false;
  errorCap = err;
    console.trace();
    console.log(err.message);
    console.log(err.stack);
    console.log(err.options.headers['Operation-Id']);
    throw err;
};

String.prototype.format = function(args) {
  var str = this;
  return str.replace(String.prototype.format.regex, function(item) {
    var intVal = parseInt(item.substring(1, item.length - 1));
    var replace;
    if (intVal >= 0) {
      replace = args[intVal];
    } else if (intVal === -1) {
      replace = '{';
    } else if (intVal === -2) {
      replace = '}';
    } else {
      replace = '';
    }
    return replace;
  });
};
String.prototype.format.regex = new RegExp('{-?[0-9]+}', 'g');

module.exports = {
  AuthClient: function(url) {
    this.url = url; 
    this.token = '';
    this.tokenExpires = '';

    // returns an access token
    this.getToken = function(clientId, clientSecret, resource) {
      return restCall({
        url: resource + '/identity/.well-known/openid-configuration',
        method: 'GET',
        headers: {
          Accept: 'application/json'
        },
        gzip: true
      })
        .then(function(res) {
          var obj = JSON.parse(res);
          authority = obj.token_endpoint;

          return restCall({
            url: authority,
            method: 'POST',
            headers: {
              'Content-Type': 'application/x-www-form-urlencoded'
            },
            form: {
              grant_type: 'client_credentials',
              client_id: clientId,
              client_secret: clientSecret,
              resource: resource
            },
            gzip: true
          });
        })
        .catch(function(err) {
          logError(err);
        });
    };

  }
};
