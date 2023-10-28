import { ApiBase, fetchApi } from "@/app/api"
import { Box, Card, CardActionArea, CardContent, CardMedia, Grid, LinearProgress, Pagination, Stack, Typography } from "@mui/material"
import { useEffect, useState } from "react"

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

    const [products, setProducts] = useState<any | undefined>(null)

    useEffect(() => {
        fetchApi(ApiBase.Main, "/products")
        .then((payload) => setProducts(payload))
    }, [])

    return (
        <Box
            sx={{
                m: 2
            }}>
                {
                    products == null ?
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
                                                                        m: 0,
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
                                        size={'large'}
                                        count={10}
                                        variant={'outlined'} />
                            </Stack>
                        </Box>
                }
        </Box>
    )
}