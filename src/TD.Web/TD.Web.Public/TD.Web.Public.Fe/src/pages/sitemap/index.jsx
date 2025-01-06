import { Button, Stack, Typography } from '@mui/material'
import { handleApiError, webApi } from '../../api/webApi'
import Link from 'next/link'

export async function getServerSideProps({ res }) {
    let products
    await webApi
        .get('/products?pageSize=10000')
        .then(async (response) => (products = response.data.payload))
        .catch((err) => {
            products = []
            handleApiError(err)
        })

    return {
        props: {
            products,
        },
    }
}

export const SitemapPage = ({ products }) => {
    return (
        <Stack textAlign={`center`} spacing={2}>
            <Typography component={`h1`}>Sitemap</Typography>
            <Typography>
                XML file can be found at{' '}
                <Link href={`/sitemap.xml`}>/sitemap.xml</Link>
            </Typography>
            {products.map((product) => (
                <Button key={product.id} href={`/proizvodi/${product.src}`}>
                    {product.src}
                </Button>
            ))}
        </Stack>
    )
}

export default SitemapPage
