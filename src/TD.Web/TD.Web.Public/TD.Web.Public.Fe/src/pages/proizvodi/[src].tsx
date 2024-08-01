import { CenteredContentWrapper } from '@/widgets/CenteredContentWrapper'
import {
    Button,
    Card,
    CardMedia,
    CircularProgress,
    Divider,
    Grid,
    Stack,
    Typography,
    styled,
} from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { ApiBase, ContentType, fetchApi } from '@/app/api'
import StandardSvg from '@/assets/Standard.svg'
import HobiSvg from '@/assets/Hobi.svg'
import ProfiSvg from '@/assets/Profi.svg'
import { toast } from 'react-toastify'
import useCookie from 'react-use-cookie'
import {
    CookieNames,
    ProizvodSrcDescription,
    ProizvodSrcTitle,
} from '@/app/constants'
import { useUser } from '@/app/hooks'
import { OneTimePrice } from '@/widgets/Proizvodi/ProizvodiSrc/OneTimePrice'
import { UserPrice } from '@/widgets/Proizvodi/ProizvodiSrc/UserPrice'
import { CustomHead } from '@/widgets/CustomHead'
import parse from 'html-react-parser'
import { SuggestedProducts } from '@/widgets'
import { KolicineInput } from '@/widgets/Proizvodi/ProizvodiSrc/KolicineInput/KolicineInput'

export async function getServerSideProps(context: any) {
    let obj = { props: {} }
    await fetchApi(
        ApiBase.Main,
        `/products/${context.params.src}`,
        undefined,
        context.req?.headers?.cookie
            ?.split(';')
            .map((cookie: string) => {
                let parts = cookie.split('=')
                return {
                    key: parts[0],
                    value: parts[1],
                }
            })
            .find((cookie: any) => cookie.key == 'token')?.value
    )
        .then(async (payload: any) => {
            await payload.json().then((payload: any) => {
                obj.props = { product: payload }
            })
        })
        .catch((error: any) => {
            console.error(error)
        })

    return obj
}

const ProizvodiSrc = (props: any): JSX.Element => {
    const router = useRouter()
    const user = useUser(false, true)

    const [productImage, setProductImage] = useState<string | undefined>(
        'data:image/jpeg;base64,' + props.product.imageData.data
    )
    const [product, setProduct] = useState<any>(props.product)

    console.log(product)

    const [baseKolicina, setBaseKolicina] = useState<number | null>(null)
    const [altKolicina, setAltKolicina] = useState<number | null>(null)

    const [isAddingToCart, setIsAddingToCart] = useState<boolean>(false)

    const [cartId, setCartId] = useCookie(CookieNames.cartId, undefined)

    useEffect(() => {
        if (product == null) return
        setBaseKolicina(1)
        setAltKolicina(product.oneAlternatePackageEquals)
    }, [product])

    return (
        <CenteredContentWrapper>
            <CustomHead
                title={product.metaTitle ?? ProizvodSrcTitle(product?.title)}
                description={
                    product.metaDescription ??
                    ProizvodSrcDescription(product?.shortDescription)
                }
            />
            <Stack p={2}>
                <Stack direction={`row`} m={2}>
                    <Button
                        variant={`contained`}
                        onClick={() => {
                            router.back()
                        }}
                    >
                        Nazad
                    </Button>
                </Stack>
                <Grid
                    container
                    direction={`row`}
                    justifyContent={`center`}
                    spacing={4}
                >
                    <Grid
                        item
                        sm={6}
                        container
                        justifyContent={`center`}
                        alignContent={`center`}
                    >
                        {productImage == undefined ? (
                            <CircularProgress />
                        ) : (
                            <Card>
                                <CardMedia
                                    sx={{
                                        objectFit: 'contain',
                                    }}
                                    component={'img'}
                                    image={productImage}
                                />
                            </Card>
                        )}
                    </Grid>
                    <Grid item sm={4}>
                        <Stack spacing={2}>
                            <Typography
                                variant="h5"
                                component="h1"
                                fontWeight={`bolder`}
                            >
                                {product?.title}
                            </Typography>
                            <Typography variant="body1" component="p">
                                {product?.shortDescription}
                            </Typography>
                            <Grid>
                                <Cene
                                    isWholesale={product?.isWholesale}
                                    userPrice={product?.userPrice}
                                    oneTimePrice={product?.oneTimePrice}
                                    unit={product?.unit}
                                    vat={product?.vat}
                                />
                                <KolicineInput
                                    baseKolicina={baseKolicina}
                                    altKolicina={altKolicina}
                                    baseUnit={product?.unit}
                                    altUnit={product?.alternateUnit}
                                    oneAlternatePackageEquals={
                                        product?.oneAlternatePackageEquals
                                    }
                                    onBaseKolicinaValueChange={(
                                        val: number
                                    ) => {
                                        setBaseKolicina(
                                            parseFloat(val.toFixed(3))
                                        )
                                        if (
                                            product?.oneAlternatePackageEquals !=
                                            null
                                        )
                                            setAltKolicina(
                                                parseFloat(
                                                    (
                                                        val *
                                                        product?.oneAlternatePackageEquals
                                                    ).toFixed(3)
                                                )
                                            )
                                    }}
                                />
                                <Button
                                    disabled={isAddingToCart}
                                    startIcon={
                                        isAddingToCart ? (
                                            <CircularProgress size={`1em`} />
                                        ) : null
                                    }
                                    variant={`contained`}
                                    sx={{ width: `100%`, my: 2 }}
                                    onClick={() => {
                                        setIsAddingToCart(true)
                                        fetchApi(
                                            ApiBase.Main,
                                            `/products/${product?.id}/add-to-cart`,
                                            {
                                                method: 'PUT',
                                                body: {
                                                    id: product.id,
                                                    quantity:
                                                        altKolicina ??
                                                        baseKolicina,
                                                    oneTimeHash: cartId,
                                                },
                                                contentType:
                                                    ContentType.ApplicationJson,
                                            }
                                        )
                                            .then((payload: any) => {
                                                payload
                                                    .text()
                                                    .then((cartId: string) => {
                                                        toast.success(
                                                            'Proizvod je dodat u korpu'
                                                        )
                                                        setCartId(cartId)
                                                        router.push('/korpa')
                                                    })
                                            })
                                            .finally(() => {
                                                setIsAddingToCart(false)
                                            })
                                    }}
                                >
                                    Dodaj u korpu
                                </Button>
                            </Grid>
                            <Divider />
                            <Stack spacing={0}>
                                <AdditionalInfoSpanText
                                    text={`Kataloški broj:`}
                                />
                                <AdditionalInfoMainText
                                    text={product?.catalogId}
                                />
                                <AdditionalInfoSpanText text={`Kategorije:`} />
                                {product?.category.map(
                                    (cat: any, index: number) => {
                                        return (
                                            <Typography key={index}>
                                                <AdditionalInfoMainText
                                                    text={formatCategory(cat)}
                                                />
                                            </Typography>
                                        )
                                    }
                                )}
                                <AdditionalInfoSpanText text={`JM:`} />
                                <AdditionalInfoMainText text={product?.unit} />
                            </Stack>
                            <Divider />
                        </Stack>
                    </Grid>
                    <Grid item sm={2}>
                        <Card
                            sx={{
                                border: 'none',
                                boxShadow: 'none',
                                backgroundColor: 'transparent',
                            }}
                        >
                            <CardMedia
                                sx={{
                                    objectFit: 'contain',
                                }}
                                component={'img'}
                                image={
                                    product?.classification == '1'
                                        ? StandardSvg.src
                                        : product?.classification == '0'
                                        ? HobiSvg.src
                                        : ProfiSvg.src
                                }
                            />
                        </Card>
                    </Grid>
                </Grid>
                <SuggestedProducts baseProductId={product?.id} />
                <FullDescriptionStyled>
                    {product.fullDescription && parse(product?.fullDescription)}
                </FullDescriptionStyled>
            </Stack>
        </CenteredContentWrapper>
    )
}

const FullDescriptionStyled = styled(Grid)(
    ({ theme }) => `
        margin: ${theme.spacing(4)} 0;
        padding: 0;
        max-width: calc(100vw - ${theme.spacing(2 * 2)});
        overflow-x: auto;

        table {
            width: 100%;
            border-spacing: 0;
        }

        td, th {
            text-align: center;
            padding: ${theme.spacing(1)};
            border-bottom: 1px solid ${theme.palette.grey[500]};
        }
    `
)

const formatCategory = (category: any): string => {
    if (category.child == null) return category.name

    return category.name + ` > ` + formatCategory(category.child)
}

const Cene = (props: any): JSX.Element => {
    return props.userPrice == null ? (
        <OneTimePrice
            data={{
                isWholesale: props.isWholesale,
                oneTimePrice: props.oneTimePrice,
                unit: props.unit,
                vat: props.vat,
            }}
        />
    ) : (
        <UserPrice
            data={{
                isWholesale: props.isWholesale,
                userPrice: props.userPrice,
                unit: props.unit,
            }}
        />
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
                marginRight: 1,
            }}
        >
            {props.text}
        </Typography>
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
                marginRight: 1,
            }}
        >
            {props.text}
        </Typography>
    )
}

export default ProizvodiSrc
