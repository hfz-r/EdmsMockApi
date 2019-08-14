var path = require('path');

module.exports = {
  build: {
    srcDir: path.resolve(__dirname, '../src'),
    distDir: path.resolve(__dirname, '../../wwwroot/dist'),
    assetsRoot: path.resolve(__dirname, '../../wwwroot/dist'),
    assetsSubDirectory: 'static',
    assetsPublicPath: '/dist/'
  },
  dev: {
    assetsSubDirectory: 'static',
    assetsPublicPath: '/dist/'
  }
};
