import { Box, Card, CardActionArea, CardContent, CardMedia, Grid, LinearProgress, Pagination, Stack, Typography } from "@mui/material"
import { useState } from "react"

export const ProizvodiList = (): JSX.Element => {

    const hobiBorderColor = 'gray'
    const standardBorderColor = 'green'
    const profiBorderColor = 'orange'

    const [products, setProducts] = useState<any | undefined>([
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-ploca',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 0
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt-1',
            src: 'gips-ploca-1',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 0
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-ploca-2',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 0
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-ploca-3',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-ploca-4',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-ploca-5',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-ploca-51',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-ploca-521',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-ploca-5vsa21',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-plocavsa-521',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-plocfsasafa-521',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-pbsasabloca-521',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-plobsaasbbsaca-521',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-psbasabsabloca-521',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-plbsasbasaboca-521',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gips-bsasabsabasbbsa-521',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'bsasbasab-ploca-521',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
        {
            imageSrc: 'https://termodom.rs/img/gallery/128/GKP-STANDARD04.jpg',
            imageAlt: 'some-alt',
            src: 'gisvavsavsaps-ploca-521',
            title: 'Gipsploca 12.5mm 1.20 * 2.0',
            classification: 1
        },
    ])

    return (
        <Box
            sx={{
                my: 2
            }}>
                {
                    products == null ?
                        <LinearProgress /> :
                        <Box>
                            <Grid
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
                                                            borderColor: standardBorderColor
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
                                                                    variant={'body1'}>Ovo je neki naslov</Typography>
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