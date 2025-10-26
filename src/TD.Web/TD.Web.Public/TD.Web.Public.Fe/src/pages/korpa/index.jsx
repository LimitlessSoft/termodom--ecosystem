import { KorpaDiscountAlert } from '@/widgets/Korpa/KorpaContent/ui/KorpaDiscountAlert'
import { KorpaSummary } from '@/widgets/Korpa/KorpaContent/ui/KorpaSummary'
import { CookieNames, KorpaTitle, UIDimensions } from '@/app/constants'
import {
    KorpaContent,
    KorpaContinueShoppingButton,
    KorpaContinueToTheOrderButton,
} from '@/widgets/Korpa/KorpaContent'
import { KorpaEmpty } from '@/widgets/Korpa/KorpaEmpty'
import { Alert, Box, Grid, LinearProgress, Stack } from '@mui/material'
import { HorizontalActionBarButton } from '@/widgets/TopActionBar'
import { CustomHead } from '@/widgets/CustomHead'
import { useEffect, useRef, useState } from 'react'
import useCookie from 'react-use-cookie'
import { useRouter } from 'next/router'
import { useUser } from '@/app/hooks'
import { handleApiError, webApi } from '@/api/webApi'

const Korpa = () => {
    const user = useUser(false, true)
    const [cartId] = useCookie(CookieNames.cartId)
    const [cart, setCart] = useState(null)
    const summaryRef = useRef()
    const router = useRouter()

    const [discountOffset, setDiscountOffset] = useState(0)

    const handleScroll = () => {
        if (summaryRef.current) {
            setDiscountOffset(summaryRef.current.offsetHeight)
        }
    }

    useEffect(() => {
        window.addEventListener('scroll', handleScroll)

        return () => {
            window.removeEventListener('scroll', handleScroll)
        }
    }, [])

    const ucitajKorpu = (cartId) => {
        webApi
            .get(`/cart?oneTimeHash=${cartId}`)
            .then((res) => setCart(res.data))
            .catch((err) => handleApiError(err))
    }

    const reloadInterval = 1000 * 60 * 5

    useEffect(() => {
        if (!cartId) return

        ucitajKorpu(cartId)

        const interval = setInterval(() => {
            ucitajKorpu(cartId)
        }, reloadInterval)

        return () => clearInterval(interval)
    }, [reloadInterval, cartId])

    return !cart && cartId ? (
        <Box>
            <CustomHead title={KorpaTitle} />
            <LinearProgress />
        </Box>
    ) : !cartId || cart.items.length === 0 ? (
        <KorpaEmpty />
    ) : (
        <Box maxWidth={UIDimensions.maxWidth} margin={`auto`}>
            <Box
                sx={{
                    display: 'grid',
                    gridTemplateColumns: {
                        xs: '1fr',
                        md: '2fr 1fr',
                    },
                    gridTemplateRows: 'auto 1fr',
                    gap: '1rem',
                    my: '2rem',
                    px: '1rem',
                    maxWidth: 'calc(100vw - 1rem)',
                    position: 'relative',
                }}
            >
                <CustomHead title={KorpaTitle} />

                <Grid container>
                    <HorizontalActionBarButton
                        text={`Nastavi kupovinu`}
                        onClick={() => {
                            router.push(`/`)
                        }}
                    />
                </Grid>
                <Stack
                    gap={2}
                    sx={{
                        gridColumn: { md: '1 / 3', xs: 'initial' },
                        gridRow: { md: '2', xs: 'initial' },
                    }}
                >
                    <Alert severity="info" variant={`filled`}>
                        Povećajte ukupnu vrednost korpe za veći popust!
                    </Alert>
                    {/*<Alert severity="success" variant={`filled`}>*/}
                    {/*    Vaša korpa prelazi visoku vrednost te su Vam odobreni*/}
                    {/*    dodatni rabati u odnosu na cenu koju ste videli u samom*/}
                    {/*    proizvodu!*/}
                    {/*</Alert>*/}
                </Stack>
                <Stack
                    sx={{
                        gridColumn: { md: '1 / 2', xs: 'initial' },
                        gridRow: { md: '3 / span 5', xs: 'initial' },
                        overflow: 'auto',
                        gap: 2,
                    }}
                >
                    <KorpaContent
                        cart={cart}
                        reloadKorpa={() => ucitajKorpu(cartId)}
                        onItemRemove={(it) => {
                            setCart((prev) => ({
                                ...prev,
                                items: prev.items.filter((i) => i.id !== it.id),
                            }))
                        }}
                    />
                    {/* Cart actions buttons */}
                    <Stack
                        gap={2}
                        sx={{
                            width: 'max-content',
                            '& a, & .MuiButton-root': {
                                fontSize: {
                                    xs: '0.7rem',
                                    md: '0.875rem',
                                },
                            },
                        }}
                    >
                        <KorpaContinueShoppingButton />
                    </Stack>
                </Stack>
                {!user.isLogged && cart && (
                    <Box
                        sx={{
                            gridColumn: { md: '2 / 3', xs: 'initial' },
                            gridRow: { md: '3', xs: 'initial' },
                            position: { xs: 'initial', md: 'sticky' },
                            top: { xs: 'initial', md: '1rem' },
                        }}
                    >
                        <KorpaDiscountAlert
                            cartId={cartId}
                            valueWithoutVAT={cart.summary.valueWithoutVAT}
                            reloadInterval={reloadInterval}
                        />
                    </Box>
                )}
                <Box
                    sx={{
                        gridColumn: { md: '2 / 3', xs: 'initial' },
                        gridRow: { md: '4', xs: 'initial' },
                        position: 'sticky',
                        bottom: { xs: '1rem', md: 'initial' },
                        top: {
                            xs: 'initial',
                            md: `calc(${discountOffset + 'px'} + 1rem)`,
                        },
                    }}
                    ref={summaryRef}
                >
                    <Stack gap={2}>
                        <KorpaSummary cart={cart} />
                        <KorpaContinueShoppingButton
                            sx={{
                                display: {
                                    md: 'block',
                                    xs: 'none',
                                },
                                width: '100%',
                                textAlign: 'center',
                            }}
                        />
                        <KorpaContinueToTheOrderButton />
                    </Stack>
                </Box>
            </Box>
        </Box>
    )
}

export default Korpa
