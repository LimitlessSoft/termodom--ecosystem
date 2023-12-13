import { CenteredContentWrapper } from "@/widgets/CenteredContentWrapper"
import { Box, Button, Card, CardActionArea, CardMedia, Divider, Grid, Stack, Typography } from "@mui/material"
import { useRouter } from "next/router"
import { useEffect, useState } from "react"
import { ApiBase, fetchApi } from "@/app/api"
import StandardSvg from './assets/standard.svg'
import HobiSvg from './assets/hobi.svg'
import ProfiSvg from './assets/profi.svg'

const ProizvodiSrc = (): JSX.Element => {
    
    const router = useRouter()
    const productSrc = router.query.src
    const [someImage, setSomeImage] = useState<string>('')
    const [product, setProduct] = useState<any | undefined>(undefined)

    useEffect(() => {
        if(productSrc == undefined)
            return

        fetchApi(ApiBase.Main, `/products/${productSrc}`)
        .then((payload: any) => {
            setProduct(payload)
            setSomeImage('data:image/jpeg;base64,' + payload.imageData.data)
        })
    }, [productSrc])

    return (
        <CenteredContentWrapper>
            <Stack>
                <Stack
                    direction={`row`}
                    m={2}>
                    <Button
                        variant={`contained`}
                        onClick={() => {
                            router.back()
                        }}>Nazad</Button>
                </Stack>
                <Grid
                    container
                    direction={`row`}
                    spacing={4}>
                        <Grid item
                            sm={6}>
                            <Card>
                                <CardMedia
                                    sx={{
                                        objectFit: 'contain'
                                    }}
                                    component={'img'}
                                    image={someImage} />
                            </Card>
                        </Grid>
                        <Grid item
                            sm={4}>
                            <Stack
                                spacing={2}>
                                <Typography
                                    variant="h5"
                                    component="h1">
                                    {product?.title}
                                </Typography>
                                <Typography
                                    variant="body1"
                                    component="p">
                                    {product?.shortDescription}
                                </Typography>
                                <Grid>
                                    Price, quantity & add to cart
                                </Grid>
                                <Divider />
                                <Stack spacing={0}>
                                    <Typography>
                                        <AdditionalInfoSpanText text={`Kataloški broj:`} />
                                        <AdditionalInfoMainText text={product?.catalogId} />
                                    </Typography>
                                    <Typography>
                                        <AdditionalInfoSpanText text={`Kategorija:`} />
                                        <AdditionalInfoMainText text={product?.category} />
                                    </Typography>
                                    <Typography>
                                        <AdditionalInfoSpanText text={`JM:`} />
                                        <AdditionalInfoMainText text={product?.unit} />
                                    </Typography>
                                </Stack>
                                <Divider />
                            </Stack>
                        </Grid>
                        <Grid item
                            sm={2}>
                            <Card
                                sx={{
                                    border: 'none',
                                    boxShadow: 'none',
                                    backgroundColor: 'transparent' }}>
                                <CardMedia
                                    sx={{
                                        objectFit: 'contain',
                                    }}
                                    component={'img'}
                                    image={
                                        product?.classification == '1' ?
                                            StandardSvg.src :
                                            product?.classification == '0' ?
                                                HobiSvg.src :
                                                ProfiSvg.src
                                                } />
                            </Card>
                        </Grid>
                </Grid>
            </Stack>
        </CenteredContentWrapper>
    )
}

const AdditionalInfoSpanText = (props: any): JSX.Element => {
    const additionalInfoSpanTextColor = 'gray'
    return (
        <Typography
            component={`span`}
            sx={{
                color: additionalInfoSpanTextColor,
                fontWeight: 'bold',
                marginRight: 1
            }}>{props.text}</Typography>
    )
}

const AdditionalInfoMainText = (props: any): JSX.Element => {
    const additionalInfoMainTextColor = '#333'
    return (
        <Typography
            component={`span`}
            sx={{
                color: additionalInfoMainTextColor,
                fontWeight: 'bold',
                marginRight: 1
            }}>{props.text}</Typography>
    )
}

export default ProizvodiSrc