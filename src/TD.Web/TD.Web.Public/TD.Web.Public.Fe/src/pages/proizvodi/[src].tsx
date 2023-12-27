import { CenteredContentWrapper } from "@/widgets/CenteredContentWrapper"
import { Box, Button, Card, CardActionArea, CardMedia, Divider, Grid, Input, LinearProgress, Stack, TextField, Typography, styled } from "@mui/material"
import { useRouter } from "next/router"
import { useEffect, useState } from "react"
import { ApiBase, fetchApi } from "@/app/api"
import StandardSvg from './assets/Standard.svg'
import HobiSvg from './assets/Hobi.svg'
import ProfiSvg from './assets/Profi.svg'
import { KolicinaInput } from "@/widgets/KolicinaInput"
import { HorizontalSplit } from "@mui/icons-material"

const ProizvodiSrc = (): JSX.Element => {
    
    const router = useRouter()
    const productSrc = router.query.src
    const [someImage, setSomeImage] = useState<string>('')
    const [product, setProduct] = useState<any | undefined>(undefined)

    const [baseKolicina, setBaseKolicina] = useState<number | null>(null)
    const [altKolicina, setAltKolicina] = useState<number | null>(null)

    useEffect(() => {
        setBaseKolicina(1)
    }, [product])

    useEffect(() => {
        if(productSrc == undefined)
            return

        fetchApi(ApiBase.Main, `/products/${productSrc}`)
        .then((payload: any) => {
            setProduct(payload)
            setSomeImage('data:image/jpeg;base64,' + payload.imageData.data)
        })
    }, [productSrc])

    useEffect(() => {
    }, [baseKolicina])

    useEffect(() => {
    }, [altKolicina])

    return (
        product == null ?
            <LinearProgress /> :
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
                                sm={6}
                                container
                                alignContent={`center`}>
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
                                        component="h1"
                                        fontWeight={`bolder`}>
                                        {product?.title}
                                    </Typography>
                                    <Typography
                                        variant="body1"
                                        component="p">
                                        {product?.shortDescription}
                                    </Typography>
                                    <Grid>
                                        <Cene userPrice={product?.userPrice} oneTimePrice={product?.oneTimePrice} unit={product?.unit} />
                                        <KolicineInput
                                            baseKolicina={baseKolicina}
                                            altKolicina={altKolicina}
                                            baseUnit={product?.unit}
                                            altUnit={product?.alternateUnit}
                                            setBaseKolicina={setBaseKolicina}
                                            setAltKolicina={setAltKolicina} />
                                            <Button
                                                variant={`contained`}
                                                sx={{ width: `100%`, my: 2 }}
                                                onClick={() => {
                                                    console.log(baseKolicina)
                                                    console.log(altKolicina)
                                                }}>Dodaj u korpu</Button>
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
                                            <AdditionalInfoMainText text={product?.baseUnit} />
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

const Cene = (props: any): JSX.Element => {
    return props.userPrice == null ?
        <OneTimePrice oneTimePrice={props.oneTimePrice} unit={props.unit} /> :
        <UserPrice userPrice={props.userPrice} unit={props.unit} />
}

const OneTimePrice = (props: any): JSX.Element => {
    return (
        <Grid container textAlign={`center`} my={3}>
            <Grid item sm={12}>
                <Grid>
                    <Typography
                        fontSize={`1.5em`}
                        variant={`h6`}
                        component={`h2`}>
                        <Typography component={`span`} sx={{ marginRight: `5px`, fontSize: `0.6em` }}>
                            MP Cena: {props.unit}
                        </Typography>
                        {props.oneTimePrice.minPrice.toFixed(2)} - {props.oneTimePrice.maxPrice.toFixed(2)}
                        <Typography component={`span`} sx={{ marginLeft: `5px`, fontSize: `0.6em` }}>
                            RSD/{props.unit}
                        </Typography>
                    </Typography>
                </Grid>
                <Grid my={1}>
                    <Typography component={`span`} sx={{ fontSize: `0.8em`, color: `rgb(203 148 92)` }}>
                        *mp cena zavisi od ukupne vrednosti vaše korpe. Tačnu cenu ćete videti u korpi
                    </Typography>
                </Grid>
            </Grid>
        </Grid>
    )
}

const UserPrice = (props: any): JSX.Element => {
    return (
        <Grid container textAlign={`center`} my={3}>
            <Grid item sm={6}>
                <Typography
                    variant={`h6`}
                    component={`h2`}
                    sx={{ color: `red`, borderRight: `1px solid gray` }}>
                    {props.userPrice.priceWithoutVAT.toFixed(2)}
                    <Typography component={`span`} sx={{ marginLeft: `5px`, fontSize: `0.6em` }}>
                        RSD/{props.unit}
                    </Typography>
                    <Typography>
                        cena bez PDV-a
                    </Typography>
                </Typography>
            </Grid>
            <Grid item sm={6}>
                <Typography
                    variant={`h6`}
                    component={`h2`}
                    sx={{ color: `green` }}>
                    {props.userPrice.priceWithVAT.toFixed(2)}
                    <Typography component={`span`} sx={{ marginLeft: `5px`, fontSize: `0.6em` }}>
                        RSD/{props.unit}
                    </Typography>
                    <Typography>
                        cena sa PDV-a
                    </Typography>
                </Typography>
            </Grid>
        </Grid>
    )
}

const KolicineInput = (props: any): JSX.Element => {

    useEffect(() => {
        console.log(props.baseUnit)
    }, [props.baseUnit])
    
    return (
        <Grid container
            spacing={1}
            justifyContent={`center`}
            sx={{ width: '100%', py: 2 }}>
                <InnerKolicinaInput value={props.baseKolicina} setKolicina={props.setBaseKolicina} unit={props.baseUnit} />
                {
                    props.altUnit == null ?
                        null :
                        <InnerKolicinaInput value={props.altKolicina} setKolicina={props.setAltKolicina} unit={props.altUnit} />
                }
        </Grid>
    )
}

const InnerKolicinaInput = (props: any): JSX.Element => {
    return (
        <Grid item
            sm={6}>
            <KolicinaInput
                value={props.value}
                unit={props.unit}
                onValueChange={(val: number) => {
                    props.setKolicina(val)
                }}
            />
        </Grid>
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