module.exports = {
    devServer: {
        proxy: {
            '/api': {
                target: 'http://139.155.183.247:22233/api',
                secure: false,
                changeOrigin: true,
                pathRewrite: {
                    '^/api': ''
                }
            }
        }
    }
}