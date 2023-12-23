import { ApiBase, fetchApi } from "@/app/api"
import { Box, Card, CardActionArea, CardContent, CardMedia, Grid, LinearProgress, Pagination, Stack, Typography } from "@mui/material"
import { useEffect, useState } from "react"
import NextLink from 'next/link'
import { useRouter } from "next/router"

const getClassificationColor = (classification: number) => {

    const hobiBorderColor = 'gray'
    const standardBorderColor = 'green'
    const profiBorderColor = 'orange'

    switch(classification) {
        case 0:
            return hobiBorderColor
        case 2:
            return profiBorderColor
        default:
            return standardBorderColor
    }
}
export const ProizvodiList = (): JSX.Element => {

    const router = useRouter()

    const pageSize = 20

    const [pagination, setPagination] = useState<any | undefined>(null)
    const [products, setProducts] = useState<any | undefined>(null)
    const [currentPage, setCurrentPage] = useState<number>(1)

    useEffect(() => {
        var cp = router.query.page
        setCurrentPage(cp == null ? 1 : parseInt(cp.toString()))
    }, [router])

    useEffect(() => {
        fetchApi(ApiBase.Main, `/products?pageSize=${pageSize}&currentPage=${currentPage}`, undefined, true)
        .then((response: any) => {
            setProducts(response.payload)
            setPagination(response.pagination)
        })
    }, [currentPage])

    return (
        <Box
            sx={{
                m: 2
            }}>
                {
                    products == null || pagination == null ?
                        <LinearProgress /> :
                        <Box>
                            <Grid
                                justifyContent={'center'}
                                container
                                spacing={2}>
                                    {
                                        products.map((p: any) => {
                                            return (
                                                <Grid
                                                    component={NextLink}
                                                    sx={{
                                                        textDecoration: 'none',
                                                    }}
                                                    href={`/proizvodi/${p.src}`}
                                                    key={`product-card-${p.src}`}
                                                    item>
                                                    <Card
                                                        sx={{
                                                            width: 190,
                                                            border: 'solid',
                                                            borderColor: getClassificationColor(p.classification)
                                                        }}>
                                                        <CardActionArea>
                                                            <CardMedia
                                                                sx={{ objectFit: 'contain'}}
                                                                component={'img'}
                                                                height={170}
                                                                image={'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg'}
                                                                alt='some-alt' />
                                                            <CardContent
                                                                sx={{
                                                                    p: 1,
                                                                    '&:last-child': {
                                                                        paddingBottom: 1
                                                                    }
                                                                }}>
                                                                <Typography
                                                                    textAlign={'center'}
                                                                    sx={{
                                                                        m: 0
                                                                    }}
                                                                    variant={'body1'}>{p.title}</Typography>
                                                            </CardContent>
                                                        </CardActionArea>
                                                    </Card>
                                                </Grid>
                                            )
                                        })
                                    }
                            </Grid>
                            <Stack
                                alignItems={'center'}
                                sx={{
                                    m: 5,
                                }}>
                                    <Pagination
                                        onChange={(event, page) => {
                                            router.push({
                                                pathname: router.pathname,
                                                query: { ...router.query, page: page.toString() }
                                            })
                                        }}
                                        size={'large'}
                                        count={Math.ceil(pagination.totalElementsCount / pagination.pageSize) }
                                        variant={'outlined'} />
                            </Stack>
                        </Box>
                }
        </Box>
    )
}