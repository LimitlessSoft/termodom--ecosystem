import { ApiBase, fetchApi } from "@/app/api"
import { Box, Card, CardActionArea, CardContent, CardMedia, CircularProgress, Grid, LinearProgress, Pagination, Stack, Typography } from "@mui/material"
import { useEffect, useState } from "react"
import NextLink from 'next/link'
import { useRouter } from "next/router"
import { useUser } from "@/app/hooks"
import { formatNumber } from "@/app/helpers/numberHelpers"

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

    const user = useUser(false, false)
    const router = useRouter()

    const pageSize = 20

    const [pagination, setPagination] = useState<any | undefined>(null)
    const [products, setProducts] = useState<any | undefined>(null)
    const [currentPage, setCurrentPage] = useState<number>(1)

    useEffect(() => {
        var cp = router.query.page
        setCurrentPage(cp == null ? 1 : parseInt(cp.toString()))
    }, [router])


    const ucitajProizvode = (page: number, grupa?: string) => {

        setProducts(null)
        let url = `/products?pageSize=${pageSize}&currentPage=${page}`
        if(grupa != null && grupa !== 'undefined' && grupa !== 'null' && grupa !== '' && grupa != undefined)
            url += `&groupName=${grupa}`

        fetchApi(ApiBase.Main, url, undefined, true)
        .then((response: any) => {
            setProducts(response.payload)
            setPagination(response.pagination)
        })
    }

    useEffect(() => {
        if(user.isLoading)
            return

        ucitajProizvode(currentPage, router.query.grupa?.toString())
    }, [user, currentPage, router.query.grupa])

    return (
        <Box
            sx={{
                width: '100%',
                m: 2
            }}>
                {
                    products == null || pagination == null ?
                        <CircularProgress /> :
                        <Box>
                            <Grid
                                justifyContent={'center'}
                                container
                                spacing={2}>
                                    {
                                        products.map((p: any) => {
                                            return <ProizvodCard key={`proizvod-card-` + p.src} proizvod={p} user={user} />
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

    const [imageData, setImageData] = useState<string | undefined>(undefined)

    useEffect(() => {
        if(props.proizvod == null) {
            setImageData(undefined)
            return
        }

        fetchApi(ApiBase.Main, `/products/${props.proizvod.src}/image?ImageQuality=100`)
        .then((payload: any) => {
            setImageData(`data:${payload.contentType};base64,${payload.data}`)
        })

    }, [props.proizvod])

    return (
        <Grid
            component={NextLink}
            sx={{
                textDecoration: 'none',
            }}
            href={`/proizvodi/${props.proizvod.src}`}
            item>
            <Card
                sx={{
                    width: 190,
                    border: 'solid',
                    borderColor: getClassificationColor(props.proizvod.classification)
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
                            <Grid>
                            <Typography
                                textAlign={'center'}
                                sx={{
                                    m: 0
                                }}
                                variant={'body1'}>{props.proizvod.title}</Typography>
                            </Grid>
                            {
                                props.user == null ?
                                    <LinearProgress /> :
                                    props.user.isLogged ?
                                        <UserPrice prices={props.proizvod.userPrice} unit={props.proizvod.unit} /> :
                                        <OneTimePrice prices={props.proizvod.oneTimePrice} unit={props.proizvod.unit} />
                            }
                    </CardContent>
                </CardActionArea>
            </Card>
        </Grid>
    )
}

const OneTimePrice = (props: any): JSX.Element => {

    const prices = props.prices

    return (
        prices == null ? <LinearProgress /> :
        <Grid
            sx={{ marginTop: `2px` }}>
            <Typography
                color={`rgb(203 148 92)`}
                variant={`caption`}>
                MP Cena /{props.unit}:
            </Typography>
            <Grid color={`green`}>
                <Typography
                    variant={`caption`}>
                        Od:
                    </Typography>
                <Typography
                    sx={{ mx: 0.5 }}
                    component={'span'}
                    variant={`subtitle2`}>
                        { formatNumber(prices.minPrice) } RSD
                    </Typography>
            </Grid>
            <Grid color={`red`}>
                <Typography
                    variant={`caption`}>
                        Do:
                    </Typography>
                <Typography
                    sx={{ mx: 0.5 }}
                    component={'span'}
                    variant={`subtitle2`}>
                        { formatNumber(prices.maxPrice) } RSD
                    </Typography>
            </Grid>
        </Grid>
    )
}

const UserPrice = (props: any): JSX.Element => {

    const prices = props.prices

    return (
        prices == null ? <LinearProgress /> :
        <Grid
            sx={{ marginTop: `2px` }}>
            <Typography
                color={`rgb(203 148 92)`}
                variant={`caption`}>
                Cena /{props.unit}:
            </Typography>
            <Grid color={`red`}>
                <Typography
                    variant={`caption`}>
                        VP Cena:
                    </Typography>
                <Typography
                    sx={{ mx: 0.5 }}
                    component={'span'}
                    variant={`subtitle2`}>
                        { formatNumber(prices.priceWithoutVAT) } RSD
                    </Typography>
            </Grid>
            <Grid color={`green`}>
                <Typography
                    variant={`caption`}>
                        VP Cena:
                    </Typography>
                <Typography
                    sx={{ mx: 0.5 }}
                    component={'span'}
                    variant={`subtitle2`}>
                        { formatNumber(prices.priceWithVAT) } RSD
                    </Typography>
            </Grid>
        </Grid>
    )
}