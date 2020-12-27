module.exports = {
    devServer: {
        proxy: {
            '/api': {
                target: 'http://localhost:12306/api',
                secure: false,
                changeOrigin: true,
                pathRewrite: {
                    '^/api': ''
                }
            }
        }
    }
}