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
import StandardSvg from '@/assets/Standard.svg'
import HobiSvg from '@/assets/Hobi.svg'
import ProfiSvg from '@/assets/Profi.svg'
import { toast } from 'react-toastify'
import useCookie, { getCookie } from 'react-use-cookie'
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
import { getServerSideWebApi, handleApiError, webApi } from '@/api/webApi'
import { CenaNaUpitSingleProductDetails } from '@/widgets/Proizvodi/ProizvodiSrc/CenaNaUpit/ui/CenaNaUpitSingleProductDetails'
import { SamoZaKupceSaUgovorom } from '@/widgets/Proizvodi/ProizvodiSrc/SamoZaKupceSaUgovorom/ui/SamoZaKupceSaUgovorom'

export async function getServerSideProps(context: any) {
    let obj = { props: {} }

    await getServerSideWebApi(context)
        .get(`/products/${context.params.src}`)
        .then((res) => (obj.props = { product: res.data }))
        .catch((err) => handleApiError(err))

    return obj
}

const ProizvodiSrc = (props: any) => {
    const router = useRouter()
    const user = useUser(false, true)

    const productImage = () =>
        'data:image/jpeg;base64,' + props.product.imageData.data

    const [baseKolicina, setBaseKolicina] = useState<number | null>(null)
    const [altKolicina, setAltKolicina] = useState<number | null>(null)

    const [isAddingToCart, setIsAddingToCart] = useState<boolean>(false)

    const [cartId, setCartId] = useCookie(CookieNames.cartId, undefined)

    useEffect(() => {
        if (props.product == null) return
        setBaseKolicina(1)
        setAltKolicina(props.product.oneAlternatePackageEquals)
    }, [props.product])

    const isPriceNaUpit =
        (props.product?.oneTimePrice !== null &&
            (props.product?.oneTimePrice.minPrice === 0 ||
                props.product?.oneTimePrice.maxPrice === null)) ||
        (props.product?.userPrice !== null &&
            props.product?.userPrice !== undefined &&
            (props.product?.userPrice.priceWithoutVAT === 0 ||
                props.product.userPrice.priceWithVAT === 0))

    return (
        <CenteredContentWrapper>
            <CustomHead
                title={
                    props.product.metaTitle ??
                    ProizvodSrcTitle(props.product?.title)
                }
                description={
                    props.product.metaDescription ??
                    ProizvodSrcDescription(props.product?.shortDescription)
                }
            />
            <Stack p={2}>
                <Stack direction={`row`} m={2}>
                    <Button
                        variant={`contained`}
                        onClick={() => {
                            router.push('/')
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
                    marginBottom={5}
                >
                    <Grid
                        item
                        sm={6}
                        container
                        justifyContent={`center`}
                        alignContent={`center`}
                    >
                        {productImage ? (
                            <Card>
                                <CardMedia
                                    sx={{
                                        objectFit: 'contain',
                                    }}
                                    component={'img'}
                                    image={productImage()}
                                />
                            </Card>
                        ) : (
                            <CircularProgress />
                        )}
                    </Grid>
                    <Grid item sm={4}>
                        <Stack spacing={2}>
                            <Typography
                                variant="h5"
                                component="h1"
                                fontWeight={`bolder`}
                            >
                                {props.product?.title}
                            </Typography>
                            <Typography variant="body1" component="p">
                                {props.product?.shortDescription}
                            </Typography>
                            {isPriceNaUpit ? (
                                <CenaNaUpitSingleProductDetails />
                            ) : (
                                <Grid>
                                    <Cene
                                        isWholesale={props.product?.isWholesale}
                                        userPrice={props.product?.userPrice}
                                        oneTimePrice={
                                            props.product?.oneTimePrice
                                        }
                                        unit={props.product?.unit}
                                        vat={props.product?.vat}
                                    />
                                    <KolicineInput
                                        disabled={
                                            isAddingToCart ||
                                            props.product?.isWholesale
                                        }
                                        baseKolicina={baseKolicina}
                                        altKolicina={altKolicina}
                                        baseUnit={props.product?.unit}
                                        altUnit={props.product?.alternateUnit}
                                        oneAlternatePackageEquals={
                                            props.product
                                                ?.oneAlternatePackageEquals
                                        }
                                        onBaseKolicinaValueChange={(
                                            val: number
                                        ) => {
                                            setBaseKolicina(
                                                parseFloat(val.toFixed(3))
                                            )
                                            if (
                                                props.product
                                                    ?.oneAlternatePackageEquals !=
                                                null
                                            )
                                                setAltKolicina(
                                                    parseFloat(
                                                        (
                                                            val *
                                                            props.product
                                                                ?.oneAlternatePackageEquals
                                                        ).toFixed(3)
                                                    )
                                                )
                                        }}
                                    />
                                    {props.product?.isWholesale && (
                                        <SamoZaKupceSaUgovorom />
                                    )}
                                    {!props.product?.isWholesale && (
                                        <Button
                                            disabled={isAddingToCart}
                                            startIcon={
                                                isAddingToCart && (
                                                    <CircularProgress
                                                        size={`1em`}
                                                    />
                                                )
                                            }
                                            variant={`contained`}
                                            sx={{ width: `100%`, my: 2 }}
                                            onClick={() => {
                                                setIsAddingToCart(true)
                                                webApi
                                                    .put(
                                                        `/products/${props.product?.id}/add-to-cart`,
                                                        {
                                                            id: props.product
                                                                .id,
                                                            quantity:
                                                                altKolicina ??
                                                                baseKolicina,
                                                            oneTimeHash: cartId,
                                                        }
                                                    )
                                                    .then((responseData) => {
                                                        toast.success(
                                                            'Proizvod je dodat u korpu'
                                                        )
                                                        setCartId(
                                                            responseData.data
                                                        )
                                                        router.push('/korpa')
                                                    })
                                                    .catch((err) =>
                                                        handleApiError(err)
                                                    )
                                                    .finally(() => {
                                                        setIsAddingToCart(false)
                                                    })
                                            }}
                                        >
                                            Dodaj u korpu
                                        </Button>
                                    )}
                                </Grid>
                            )}
                            <Divider />
                            <Stack spacing={0}>
                                <AdditionalInfoSpanText
                                    text={`Kataloški broj:`}
                                />
                                <AdditionalInfoMainText
                                    text={props.product?.catalogId}
                                />
                                <AdditionalInfoSpanText text={`Kategorije:`} />
                                {props.product?.category.map(
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
                                <AdditionalInfoMainText
                                    text={props.product?.unit}
                                />
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
                                    props.product?.classification == '1'
                                        ? StandardSvg.src
                                        : props.product?.classification == '0'
                                          ? HobiSvg.src
                                          : ProfiSvg.src
                                }
                            />
                        </Card>
                    </Grid>
                </Grid>
                <SuggestedProducts baseProductId={props.product?.id} />
                <FullDescriptionStyled>
                    {props.product.fullDescription &&
                        parse(props.product?.fullDescription)}
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

const Cene = (props: any) => {
    return props.userPrice ? (
        <UserPrice
            data={{
                isWholesale: props.isWholesale,
                userPrice: props.userPrice,
                unit: props.unit,
            }}
        />
    ) : (
        <OneTimePrice
            data={{
                isWholesale: props.isWholesale,
                oneTimePrice: props.oneTimePrice,
                unit: props.unit,
                vat: props.vat,
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
