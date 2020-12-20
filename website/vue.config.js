module.exports = {
    devServer: {
        proxy: {
            '/api': {
                target: 'http://localhost:12138/api',
                secure: false,
                changeOrigin: true,
                pathRewrite: {
                    '^/api': ''
                }
            }
        }
    }
}