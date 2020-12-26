module.exports = {
    devServer: {
        proxy: {
            '/api': {
                target: 'http://192.168.1.143:12306/api',
                secure: false,
                changeOrigin: true,
                pathRewrite: {
                    '^/api': ''
                }
            }
        }
    }
}