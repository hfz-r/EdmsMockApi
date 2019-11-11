var merge = require('webpack-merge'),
  webpack = require('webpack'),
  baseWebpackConfig = require('./webpack.base.conf'),
  Dotenv = require('dotenv-webpack');

module.exports = merge(baseWebpackConfig, {
  plugins: [
    new webpack.DefinePlugin({
      'process.env.NODE_ENV': JSON.stringify('production')
    }),
    new Dotenv(),
    new webpack.optimize.UglifyJsPlugin({
      compress: {
        warnings: false
      },
      sourceMap: false
    })
  ]
});
