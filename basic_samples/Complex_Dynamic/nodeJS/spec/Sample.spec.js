describe('Complex_Dynamic NodeJS Sample', function () {
  var Sample = require('../Sample');
  jasmine.DEFAULT_TIMEOUT_INTERVAL = 60000;

  beforeEach(function () {});

  it('should be able to complete the main method', function (done) {
    sample = Sample.app(['1,2', 'n'])
      .then(() => {
        console.log('do data checks in here');
      })
      .catch(function (err) {
        console.log(err);
      })
      .finally(function () {
        done();
      });
  });
});
