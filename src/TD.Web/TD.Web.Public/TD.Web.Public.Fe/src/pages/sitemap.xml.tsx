import { handleApiError, webApi } from '@/api/webApi'

function generateSiteMap(proizvodi: any[]) {
    return `<?xml version="1.0" encoding="UTF-8"?>
    <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
    
    <url>
        <loc>https://termodom.rs/</loc>
        <lastmod>2024-05-13T18:20:59+01:00</lastmod>
        <priority>1</priority>
    </url>
    
    <url>
        <loc>https://termodom.rs/kontakt</loc>
        <lastmod>2024-05-13T18:20:59+01:00</lastmod>
        <priority>0.8</priority>
    </url>
    
    <url>
        <loc>https://termodom.rs/logovanje</loc>
        <lastmod>2024-05-13T18:20:59+01:00</lastmod>
        <priority>0.8</priority>
    </url>
    
    <url>
        <loc>https://termodom.rs/korpa</loc>
        <lastmod>2024-05-13T18:20:59+01:00</lastmod>
        <priority>0.2</priority>
    </url>
    ${proizvodi
        .map((proizvod) => {
            return `
                <url>
                    <loc>https://termodom.rs/proizvodi/${proizvod.src}</loc>
                    <lastmod>2024-05-13T18:20:59+01:00</lastmod>
                    <priority>0.5</priority>
                </url>
            `
        })
        .join('')}

   </urlset>
 `
}

function SiteMap() {
    // getServerSideProps will do the heavy lifting
}

export async function getServerSideProps({ res }: any) {
    let products: any[]
    await webApi
        .get('/products?pageSize=10000')
        .then(async (response) => (products = response.data.payload))
        .catch((err) => {
            products = []
            handleApiError(err)
        })

    res.setHeader('Content-Type', 'text/xml')
    res.write(generateSiteMap(products!))
    res.end()

    return {
        props: {},
    }
}

export default SiteMap
