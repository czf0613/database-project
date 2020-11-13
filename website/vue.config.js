module.exports = {
    devServer: {
        proxy: {
            '/api': {
                target: 'https://xxx.xxx/api',
                secure: true,
                changeOrigin: true,
                pathRewrite: {
                    '^/api': ''
                }
            }
        }
    }
}