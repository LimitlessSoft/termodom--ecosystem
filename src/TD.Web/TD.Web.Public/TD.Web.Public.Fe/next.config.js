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
                source: `/${encodeURIComponent('porudžbine')}/:hash`,
                destination: '/porudzbine/[hash]',
            },
            {
                source: '/',
                destination: '/proizvodi'
            }
        ]
    },
}
    
module.exports = nextConfig
