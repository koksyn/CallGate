const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
//const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;
const bundleOutputDir = './wwwroot/dist';

module.exports = (env) => {
    const isDevBuild = true;//TODO !(env && env.prod); => bo nie dziala ExtractTextPlugin.extract({ use: 'css-loader?minimize' }) (widac po vue-gallery)

    return [{
        stats: { modules: false },
        context: __dirname,
        resolve: { extensions: [ '.js'] },
        entry: { 'main': './ClientApp/boot.js' },
        module: {
            rules: [
                {
                    test: /\.vue\.html$/, include: /ClientApp/, loader: 'vue-loader', options: {
                            loaders: {
                        'scss': [
                            'vue-style-loader',
                            'css-loader',
                            'sass-loader'
                        ],
                        'sass': [
                            'vue-style-loader',
                            'css-loader',
                            'sass-loader?indentedSyntax'
                        ]
                    }
                // other vue-loader options go here
            } },
                {
                    test: /\.js$/,
                    loader: 'babel-loader',
                    exclude: /node_modules/
                },
                { test: /\.css$/, use: isDevBuild ? [ 'style-loader', 'css-loader' ] : ExtractTextPlugin.extract({ use: 'css-loader?minimize' }) },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' }
            ]
        },
        output: {
            path: path.join(__dirname, bundleOutputDir),
            filename: '[name].js',
            publicPath: 'dist/'
        },
        plugins: [
//            new CheckerPlugin(),
            new webpack.DefinePlugin({
                'process.env': {
                    NODE_ENV: JSON.stringify(isDevBuild ? 'development' : 'production')
                }
            }),
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require('./wwwroot/dist/vendor-manifest.json')
            })
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
//            new webpack.SourceMapDevToolPlugin({
//                filename: '[file].map', // Remove this line if you prefer inline source maps
//                moduleFilenameTemplate: path.relative(bundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
//            })
        ] : [
            // Plugins that apply in production builds only
            //new webpack.optimize.UglifyJsPlugin(), TODO bo blad na odpaleniu webpacka
            new ExtractTextPlugin('site.css')
        ])
    }];
};
