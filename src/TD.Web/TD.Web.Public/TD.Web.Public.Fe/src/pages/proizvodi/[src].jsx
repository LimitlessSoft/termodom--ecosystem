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
import { getServerSideWebApi, handleApiError, webApi } from '@/api/webApi'
import { CenaNaUpitSingleProductDetails } from '@/widgets/Proizvodi/ProizvodiSrc/CenaNaUpit/ui/CenaNaUpitSingleProductDetails'
import { SamoZaKupceSaUgovorom } from '@/widgets/Proizvodi/ProizvodiSrc/SamoZaKupceSaUgovorom/ui/SamoZaKupceSaUgovorom'
import Image from 'next/image'
import sharp from 'sharp'

export async function getServerSideProps(context) {
    const product = await getServerSideWebApi(context)
        .get(`/products/${context.params.src}`)
        .then((res) => res.data)
        .catch(handleApiError)

    if (!product)
        return {
            notFound: true,
        }

    const resizedImage = await sharp(
        Buffer.from(product.imageData.data, 'base64')
    )
        .webp({ quality: 50 })
        .toBuffer()

    const imageData = `data:image/webp;base64,${resizedImage.toString('base64')}`

    return {
        props: {
            product: { ...product, imageData },
        },
    }
}

const ProizvodiSrc = ({ product }) => {
    console.log(product)

    const router = useRouter()
    const user = useUser(false, true)

    const [baseKolicina, setBaseKolicina] = useState(null)
    const [altKolicina, setAltKolicina] = useState(null)

    const [isAddingToCart, setIsAddingToCart] = useState(false)

    const [cartId, setCartId] = useCookie(CookieNames.cartId, undefined)

    useEffect(() => {
        if (product == null) return
        setBaseKolicina(1)
        setAltKolicina(product.oneAlternatePackageEquals)
    }, [product])

    const isPriceNaUpit =
        (product?.oneTimePrice !== null &&
            (product?.oneTimePrice.minPrice === 0 ||
                product?.oneTimePrice.maxPrice === null)) ||
        (product?.userPrice !== null &&
            product?.userPrice !== undefined &&
            (product?.userPrice.priceWithoutVAT === 0 ||
                product.userPrice.priceWithVAT === 0))

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
                        {product.imageData ? (
                            <Card>
                                <Image
                                    src={product.imageData}
                                    alt={product.title}
                                    width={500}
                                    height={500}
                                    style={{
                                        width: '100%',
                                        height: 'auto',
                                        objectFit: 'contain',
                                    }}
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
                                {product?.title}
                            </Typography>
                            <Typography variant="body1" component="p">
                                {product?.shortDescription}
                            </Typography>
                            {isPriceNaUpit ? (
                                <CenaNaUpitSingleProductDetails />
                            ) : (
                                <Grid>
                                    <Cene
                                        isWholesale={product?.isWholesale}
                                        userPrice={product?.userPrice}
                                        oneTimePrice={product?.oneTimePrice}
                                        unit={product?.unit}
                                        vat={product?.vat}
                                    />
                                    <KolicineInput
                                        disabled={
                                            isAddingToCart ||
                                            product?.isWholesale
                                        }
                                        baseKolicina={baseKolicina}
                                        altKolicina={altKolicina}
                                        baseUnit={product?.unit}
                                        altUnit={product?.alternateUnit}
                                        oneAlternatePackageEquals={
                                            product?.oneAlternatePackageEquals
                                        }
                                        onBaseKolicinaValueChange={(val) => {
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
                                    {product?.isWholesale && (
                                        <SamoZaKupceSaUgovorom />
                                    )}
                                    {!product?.isWholesale && (
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
                                                        `/products/${product?.id}/add-to-cart`,
                                                        {
                                                            id: product.id,
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
                                    text={product?.catalogId}
                                />
                                <AdditionalInfoSpanText text={`Kategorije:`} />
                                {product?.category.map((cat, index) => {
                                    return (
                                        <Typography key={index}>
                                            <AdditionalInfoMainText
                                                text={formatCategory(cat)}
                                            />
                                        </Typography>
                                    )
                                })}
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

const formatCategory = (category) => {
    if (category.child == null) return category.name

    return category.name + ` > ` + formatCategory(category.child)
}

const Cene = ({ isWholesale, userPrice, unit, oneTimePrice, vat }) => {
    return userPrice ? (
        <UserPrice
            data={{
                isWholesale: isWholesale,
                userPrice: userPrice,
                unit: unit,
            }}
        />
    ) : (
        <OneTimePrice
            data={{
                isWholesale: isWholesale,
                oneTimePrice: oneTimePrice,
                unit: unit,
                vat: vat,
            }}
        />
    )
}

const AdditionalInfoSpanText = ({ text }) => {
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
            {text}
        </Typography>
    )
}

const AdditionalInfoMainText = ({ text }) => {
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
            {text}
        </Typography>
    )
}

export default ProizvodiSrc
