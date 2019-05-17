const path = require('path'),
  webpack = require('webpack');

const WebpackNotifierPlugin = require('webpack-notifier'),
  HtmlWebpackPlugin = require('html-webpack-plugin');

const applicationFolderName = 'ClientApp';
const applicationSource = path.join(__dirname, applicationFolderName);
const applicationDest = path.join(__dirname, 'wwwroot');
const nodeModules = path.join(__dirname, 'node_modules');

module.exports = {
  entry: {
    'Vendors': path.join(applicationSource, "Vendors.ts"),
    'Application': path.join(applicationSource, "Startup.ts")
  },
  output: {
    path: applicationDest,
    filename: applicationFolderName + '/[name].bundle.js',
    chunkFilename: applicationFolderName + '/Modules/[name]/[name].bundle.js',
    publicPath: '/'
  },
  resolve: {
    modules: [applicationSource, 'node_modules'],
    extensions: ['.ts', '.js', '.json', '.css', '.html', '*']
  },
  plugins: [
    new webpack.optimize.UglifyJsPlugin(),
    new HtmlWebpackPlugin({
      template: 'index.html.tmpl',
      excludeChunks: ['Vendors'],
      minify: {
        collapseWhitespace: true, collapseInlineTagWhitespace: true, removeRedundantAttributes: true,
        removeEmptyAttributes: true, removeScriptTypeAttributes: true, removeStyleLinkTypeAttributes: true
      }
    }),
    new webpack.optimize.CommonsChunkPlugin('Common'),
    new webpack.ProvidePlugin({
      $: 'jquery',
      JQuery: 'jquery'
    }),
    new CleanWebPackPlugin(applicationDest + '/*')
  ],
  module: {
    rules: [
      { test: /\.ts$/, include: [applicationSource], loaders: ['ts.loader', 'angular-router-loader'] },
      { test: /\.html$/, include: [applicationSource], loaders: ['file-loader?publicPath=/,name=[path][name].min.[ext]', { loader: 'html-minify-loader', options: { quotes: true, dom: { lowerCaseTags: false, lowerCaseAttributeNames: false } } }] },
      { test: /\.s?css$/, include: [applicationSource, nodeModules], loaders: ['file-loader?publicPath=/,name=[path][name].min.css', 'extract-loader', 'css-loader?minimize=true', 'sass-loader' },
      { test: /\.(woff(2)?|eot|svg|ttf)(\?v=\d+\.\d+\.\d+)?$/, include: [nodeModules], loaders: ['file-loader?publicPath=,name=fonts/[name].[ext]'] }
    ]
  }
};