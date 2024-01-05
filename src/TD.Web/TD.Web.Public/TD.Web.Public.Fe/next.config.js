/** @type {import('next').NextConfig} */

const nextConfig = {
    async redirects() {
        return [
            {
                source: '/proizvodi',
                destination: '/',
                permanent: true
            }
        ]
    },
    async rewrites() {
        return [
            {
                source: '/',
                destination: '/proizvodi'
            }
        ]
    },
    publicRuntimeConfig: {
        API_BASE_URL_MAIN: process.env.PUBLIC_API_URL
    }}

module.exports = nextConfig
