// omfClient.js
//

var restCall = require('request-promise');

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
  OMFClient: function(omfURL, client, basicId = null, basicPassword= null) {

    this.tokenExpires = client.tokenExpires;
    this.getToken = client.getToken;
    this.token = client.token;
    // create a type
    this.createType = function(omfType) {
      return restCall({
        url: omfURL,
        method: 'POST',
        headers: this.getHeadersType('type'),
        body: JSON.stringify(omfType).toString()
      });
    };

    // create a container
    this.createContainer = function(omfContainer) {
      return restCall({
        url: omfURL,
        method: 'POST',
        headers: this.getHeadersType('container'),
        body: JSON.stringify(omfContainer).toString()
      });
    };

    // create data
    this.createData = function(omfContainer) {
      return restCall({
        url: omfURL,
        method: 'POST',
        headers: this.getHeadersType('data'),
        body: JSON.stringify(omfContainer).toString()
      });
    };

    this.deleteType = function(omfType) {
      return restCall({
        url: omfURL,
        method: 'POST',
        headers: this.getHeadersType('type','delete'),
        body: JSON.stringify(omfType).toString()
      });
    };

    // create a container
    this.deleteContainer = function(omfContainer) {
      return restCall({
        url: omfURL,
        method: 'POST',
        headers: this.getHeadersType('container','delete'),
        body: JSON.stringify(omfContainer).toString()
      });
    };

    this.getHeadersType = function(message_type, action = 'create') {
      if(basicId){
        return {
          'messagetype': message_type,
          'omfversion': '1.1',
          'action': action,
          'messageformat': 'json',
          'Authorization': 'Basic ' + new Buffer(basicId + ':' + basicPassword).toString('base64')
        };
      }
      else if(client){
        return {
          Authorization: 'bearer ' + client.token,
          'messagetype': message_type,
          'omfversion': '1.1',
          'action': action,
          'messageformat': 'json'
        };
      }else{
        return {
          'messagetype': message_type,
          'omfversion': '1.1',
          'action': action,
          'messageformat': 'json'
        };
      }

    };

  }
}
