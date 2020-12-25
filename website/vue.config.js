module.exports = {
    devServer: {
        proxy: {
            '/api': {
                target: 'http://192.168.1.142:12138/api',
                secure: false,
                changeOrigin: true,
                pathRewrite: {
                    '^/api': ''
                }
            }
        }
    }
}