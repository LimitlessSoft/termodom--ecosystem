import { KorpaDiscountAlert } from '@/widgets/Korpa/KorpaContent/ui/KorpaDiscountAlert'
import { KorpaSummary } from '@/widgets/Korpa/KorpaContent/ui/KorpaSummary'
import { CookieNames, KorpaTitle, UIDimensions } from '@/app/constants'
import {
    KorpaContent,
    KorpaContinueToTheOrderButton,
    KorpaContinueShoppingButton,
} from '@/widgets/Korpa/KorpaContent'
import { KorpaEmpty } from '@/widgets/Korpa/KorpaEmpty'
import {
    Alert,
    Box,
    Grid,
    LinearProgress,
    Stack,
    Typography,
} from '@mui/material'
import { CustomHead } from '@/widgets/CustomHead'
import { useEffect, useState } from 'react'
import useCookie from 'react-use-cookie'
import { useRouter } from 'next/router'
import { useUser } from '@/app/hooks'
import { handleApiError, webApi } from '@/api/webApi'
import {
    HorizontalActionBar,
    HorizontalActionBarButton,
} from '@/widgets/TopActionBar'

const Korpa = () => {
    const user = useUser(false, true)
    const [cartId] = useCookie(CookieNames.cartId)
    const [cart, setCart] = useState(null)
    const router = useRouter()

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

    return !cart ? (
        <Box>
            <CustomHead title={KorpaTitle} />
            <LinearProgress />
        </Box>
    ) : cart.items.length === 0 ? (
        <KorpaEmpty />
    ) : (
        <Stack
            maxWidth={UIDimensions.maxWidth}
            margin={`auto`}
            sx={{
                position: 'relative',
                my: 3,
                px: 2,
                gap: 2,
            }}
        >
            <CustomHead title={KorpaTitle} />

            <HorizontalActionBar>
                <HorizontalActionBarButton
                    text={`Nastavi kupovinu`}
                    onClick={() => {
                        router.push(`/`)
                    }}
                />
            </HorizontalActionBar>
            <Typography
                component={`h1`}
                variant={`h4`}
                fontWeight={`bold`}
                my={1}
            >
                Korpa za kupovinu
            </Typography>
            <Box container>
                <Alert severity="info" variant={`filled`}>
                    Povećajte ukupnu vrednost korpe za veći popust!
                </Alert>
            </Box>
            <Grid
                sx={{
                    display: 'grid',
                    gridTemplateColumns: { xs: '1fr', md: '2fr 1fr' },
                    gridTemplateRows: 'auto 1fr',
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
                {/* </Grid> */}
                {!user.isLogged && cart && (
                    <KorpaDiscountAlert
                        cartId={cartId}
                        valueWithoutVAT={cart.summary.valueWithoutVAT}
                        reloadInterval={reloadInterval}
                    />
                )}
                <Grid
                    item
                    sx={{
                        position: 'sticky',
                        bottom: {
                            xs: 10,
                            md: 'unset',
                        },
                        top: {
                            xs: 'unset',
                            md: 10,
                        },
                        height: 'min-content',
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 2,
                        gridColumn: { xs: '1', md: '2' },
                        gridRow: { xs: 'auto', md: '2' },
                    }}
                >
                    <KorpaSummary cart={cart} />
                    <KorpaContinueShoppingButton
                        sx={{
                            display: {
                                xs: 'none',
                                md: 'flex',
                            },
                        }}
                    />
                    <KorpaContinueToTheOrderButton
                        sx={{
                            boxShadow: {
                                xs: 8,
                                md: 'none',
                            },
                            border: {
                                xs: '1px solid gray',
                                md: 'none',
                            },
                        }}
                    />
                </Grid>
            </Grid>
        </Stack>
    )
}

export default Korpa
