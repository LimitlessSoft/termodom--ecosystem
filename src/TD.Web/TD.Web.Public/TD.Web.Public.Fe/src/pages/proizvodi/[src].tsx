import { CenteredContentWrapper } from "@/widgets/CenteredContentWrapper"
import { Button, Card, CardMedia, CircularProgress, Divider, Grid, LinearProgress, Stack, Typography} from "@mui/material"
import { useRouter } from "next/router"
import { useEffect, useState } from "react"
import { ApiBase, ContentType, fetchApi } from "@/app/api"
import StandardSvg from './assets/Standard.svg'
import HobiSvg from './assets/Hobi.svg'
import ProfiSvg from './assets/Profi.svg'
import { KolicinaInput } from "@/widgets/KolicinaInput"
import { toast } from "react-toastify"
import useCookie from 'react-use-cookie'
import { CookieNames, ProizvodSrcDescription, ProizvodSrcTitle, removeMultipleSpaces } from "@/app/constants"
import { useUser } from "@/app/hooks"
import { OneTimePrice } from "@/widgets/Proizvodi/ProizvodiSrc/OneTimePrice"
import { UserPrice } from "@/widgets/Proizvodi/ProizvodiSrc/UserPrice"
import { CustomHead } from "@/widgets/CustomHead"

export async function getServerSideProps(context: any) {
    let obj = { props: {} }
    await fetchApi(ApiBase.Main, `/products/${context.params.src}`, undefined, false, context.req.headers.cookie.split(';').map((cookie: string) => {
        var parts = cookie.split('=')
        return {
            key: parts[0],
            value: parts[1]
        }
    }).find((cookie: any) => cookie.key == 'token')?.value)
    .then((payload: any) => {
        obj.props = { product: payload }
    })

    return obj
}

const ProizvodiSrc = (props: any): JSX.Element => {

    const router = useRouter()
    const productSrc = router.query.src
    const user = useUser(false, true)

    const [productImage, setProductImage] = useState<string | undefined>('data:image/jpeg;base64,' + props.product.imageData.data)
    const [product, setProduct] = useState<any>(props.product)

    const [baseKolicina, setBaseKolicina] = useState<number | null>(null)
    const [altKolicina, setAltKolicina] = useState<number | null>(null)

    const [isAddingToCart, setIsAddingToCart] = useState<boolean>(false)

    const [cartId, setCartId] = useCookie(CookieNames.cartId, undefined)

    useEffect(() => {
        if(product == null)
            return
        setBaseKolicina(1)
        setAltKolicina(product.oneAlternatePackageEquals)
    }, [product])

    // const ucitajProizvod = (src: string) => {
    //     fetchApi(ApiBase.Main, `/products/${src}`)
    //     .then((payload: any) => {
    //         setProduct(payload)
    //         setProductImage('data:image/jpeg;base64,' + payload.imageData.data)
    //     })
    // }

    // useEffect(() => {
    //     if(productSrc == undefined || user.isLoading)
    //         return

    //     ucitajProizvod(productSrc.toString())
    // }, [productSrc, user])

    useEffect(() => {
        if(product?.oneAlternatePackageEquals == null || baseKolicina == null)
            return

        setAltKolicina(parseFloat((product?.oneAlternatePackageEquals * baseKolicina).toFixed(3)))
    }, [baseKolicina])

    useEffect(() => {
        if(product?.oneAlternatePackageEquals == null || altKolicina == null)
            return

        setBaseKolicina(parseFloat((altKolicina / product?.oneAlternatePackageEquals).toFixed(3)))
    }, [altKolicina])

    return (
        <CenteredContentWrapper>
            <CustomHead
                title={ProizvodSrcTitle(product?.title)}
                description={ProizvodSrcDescription(product?.shortDescription)}
                />
            <Stack
                p={2}>
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
                    justifyContent={`center`}
                    spacing={4}>
                        <Grid item
                            sm={6}
                            container
                            justifyContent={`center`}
                            alignContent={`center`}>
                                {
                                    productImage == undefined ?
                                        <CircularProgress /> :
                                        <Card>
                                            <CardMedia
                                                sx={{
                                                    objectFit: 'contain'
                                                }}
                                                component={'img'}
                                                image={productImage} />
                                        </Card>
                                }
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
                                    <Cene userPrice={product?.userPrice} oneTimePrice={product?.oneTimePrice} unit={product?.unit} vat={product?.vat} />
                                    <KolicineInput
                                        baseKolicina={baseKolicina}
                                        altKolicina={altKolicina}
                                        baseUnit={product?.unit}
                                        altUnit={product?.alternateUnit}
                                        setBaseKolicina={setBaseKolicina}
                                        setAltKolicina={setAltKolicina} />
                                        <Button
                                            disabled={isAddingToCart}
                                            startIcon={isAddingToCart ? <CircularProgress size={`1em`} /> : null}
                                            variant={`contained`}
                                            sx={{ width: `100%`, my: 2 }}
                                            onClick={() => {
                                                setIsAddingToCart(true)
                                                fetchApi(ApiBase.Main, `/products/${product?.id}/add-to-cart`, {
                                                    method: 'PUT',
                                                    body: {
                                                        id: product.id,
                                                        quantity: altKolicina ?? baseKolicina,
                                                        oneTimeHash: user.isLogged ? null : cartId
                                                    },
                                                    contentType: ContentType.ApplicationJson
                                                }).then((payload: any) => {
                                                    toast.success('Proizvod je dodat u korpu')
                                                    setCartId(payload)
                                                }).finally(() => {
                                                    setIsAddingToCart(false)
                                                    router.push('/korpa')
                                                })
                                            }}>Dodaj u korpu</Button>
                                </Grid>
                                <Divider />
                                <Stack spacing={0}>
                                    <Typography>
                                        <AdditionalInfoSpanText text={`Kataloški broj:`} />
                                        <AdditionalInfoMainText text={product?.catalogId} />
                                    </Typography>
                                    <Typography>
                                        <AdditionalInfoSpanText text={`Kategorije:`} />
                                        {
                                            product?.category.map((cat: any, index: number) => {
                                                return <Typography key={index}><AdditionalInfoMainText text={formatCategory(cat)} /></Typography>
                                            })
                                        }
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
                                    backgroundColor: 'transparent',
                                    }}>
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

const formatCategory = (category: any): string => {
    if(category.child == null)
        return category.name

    return category.name + ` > ` + formatCategory(category.child)
}

const Cene = (props: any): JSX.Element => {
    return props.userPrice == null ?
        <OneTimePrice data={{ oneTimePrice: props.oneTimePrice, unit: props.unit, vat: props.vat }} /> :
        <UserPrice data={{ userPrice: props.userPrice, unit: props.unit }} />
}

const KolicineInput = (props: any): JSX.Element => {
    
    return (
        <Grid container
            spacing={1}
            justifyContent={`center`}
            sx={{ width: '100%', py: 2 }}>
                <InnerKolicinaInput value={props.baseKolicina} setKolicina={props.setBaseKolicina} unit={props.altUnit == null ? props.baseUnit : props.altUnit}
                onPlusClick={() => {
                    props.setBaseKolicina(props.baseKolicina + 1)
                }}
                onMinusClick={() => {
                    if(props.baseKolicina <= 1)
                        return
                    props.setBaseKolicina(props.baseKolicina - 1)
                }} />
                {
                    props.altUnit == null ?
                        null :
                        <InnerKolicinaInput value={props.altKolicina} setKolicina={props.setAltKolicina} unit={props.baseUnit}
                        onPlusClick={() => {
                            props.setBaseKolicina(props.baseKolicina + 1)
                        }}
                        onMinusClick={() => {
                            if(props.baseKolicina <= 1)
                                return
                            props.setBaseKolicina(props.baseKolicina - 1)
                        }} />
                }
        </Grid>
    )
}

const InnerKolicinaInput = (props: any): JSX.Element => {
    return (
        <Grid item
            sm={6}>
            <KolicinaInput
                onPlusClick={props.onPlusClick}
                onMinusClick={props.onMinusClick}
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

function getApiBaseUrlMain(): any {
    throw new Error("Function not implemented.")
}
