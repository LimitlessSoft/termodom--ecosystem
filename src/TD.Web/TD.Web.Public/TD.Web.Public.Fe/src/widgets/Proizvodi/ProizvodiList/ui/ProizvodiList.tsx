import { ApiBase, fetchApi } from "@/app/api"
import { Box, Card, CardActionArea, CardContent, CardMedia, CircularProgress, Grid, LinearProgress, Pagination, Stack, Typography } from "@mui/material"
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
                                            return <ProizvodCard key={`proizvod-card-` + p.src} proizvod={p} />
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

const ProizvodCard = (props: any): JSX.Element => {
    
    const proizvod = props.proizvod

    const [imageData, setImageData] = useState<string | undefined>(undefined)

    useEffect(() => {
        if(proizvod == null) {
            setImageData(undefined)
            return
        }

        fetchApi(ApiBase.Main, `/products/${proizvod.src}/image`)
        .then((payload: any) => {
            setImageData(`data:${payload.contentType};base64,${payload.data}`)
        })

    }, [proizvod])

    return (
        <Grid
            component={NextLink}
            sx={{
                textDecoration: 'none',
            }}
            href={`/proizvodi/${proizvod.src}`}
            item>
            <Card
                sx={{
                    width: 190,
                    border: 'solid',
                    borderColor: getClassificationColor(proizvod.classification)
                }}>
                <CardActionArea>
                    {
                        imageData == null ?
                        <Grid container
                            sx={{ p: 2 }}
                            justifyContent={`center`}>
                            <CircularProgress />
                        </Grid> :
                            <CardMedia
                                sx={{ objectFit: 'contain'}}
                                component={'img'}
                                height={170}
                                image={imageData}
                                alt={`need-to-get-from-image-tags`} />
                    }
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
                            variant={'body1'}>{proizvod.title}</Typography>
                    </CardContent>
                </CardActionArea>
            </Card>
        </Grid>
    )
}