/** @type {import('next').NextConfig} */
const nextConfig = {
    async rewrites() {
        return [
            {
                source: `/${encodeURIComponent('podešavanja')}`,
                destination: '/podesavanja',
            },
            {
                source: `/${encodeURIComponent('porudžbine')}`,
                destination: '/porudzbine',
            },
            {
                source: `/${encodeURIComponent('porudžbine')}/:id`,
                destination: '/porudzbine/[id]',
            }
        ]
    }
}

module.exports = nextConfig
