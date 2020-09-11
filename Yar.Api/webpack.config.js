const path = require("path");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

module.exports = {
    entry: {
        reader: "./assets/js/reader/reader.ts",
        texts: "./assets/js/texts/texts.ts",
    },
    output: {
        path: path.resolve(__dirname, "wwwroot/js/"),
        filename: "[name].js",
        publicPath: "/",
        libraryTarget: 'var',
        library: [
            "GlobalAccess", "[name]"
        ]
    },
    resolve: {
        extensions: [".ts"],
        alias: {
            'vue$': 'vue/dist/vue.esm.js' // 'vue/dist/vue.common.js' for webpack 1
        }
    },
    devtool: 'source-map',
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: "ts-loader"
            }
        ]
    },
    plugins: [
        new CleanWebpackPlugin()
    ]
};