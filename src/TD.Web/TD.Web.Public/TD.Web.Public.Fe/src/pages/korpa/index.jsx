import { KorpaDiscountAlert } from '@/widgets/Korpa/KorpaContent/ui/KorpaDiscountAlert'
import { KorpaZakljucivanje } from '@/widgets/Korpa/KorpaContent/ui/KorpaZakljucivanje'
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
    Button,
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
    const [cartId, setCartId] = useCookie(CookieNames.cartId)
    const [cart, setCart] = useState(null)
    const router = useRouter()
    const [contentDisabled, setContentDisabled] = useState(false)

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
                my: 4,
                px: 2,
                height: 'auto',
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
            <Grid container spacing={2}>
                <Grid
                    item
                    xs={12}
                    md={8}
                    sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}
                >
                    <KorpaContent
                        elementsDisabled={contentDisabled}
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
                </Grid>
                <Grid
                    item
                    xs={12}
                    md={4}
                    sx={{
                        position: {
                            xs: 'unset',
                            md: 'sticky',
                        },
                        top: { xs: 'unset', md: 10 },
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 2,
                    }}
                >
                    {!user.isLogged && cart && (
                        <KorpaDiscountAlert
                            cartId={cartId}
                            valueWithoutVAT={cart.summary.valueWithoutVAT}
                            reloadInterval={reloadInterval}
                        />
                    )}
                    <Stack
                        gap={2}
                        sx={{
                            position: { xs: 'sticky', md: 'unset' },
                            bottom: {
                                xs: 10,
                                md: 'unset',
                            },
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
                        // sx={{
                        //     display: { xs: 'none', sm: 'flex' },
                        // }}
                        />
                    </Stack>
                </Grid>
                {/* <KorpaContinueToTheOrderButton
                    sx={{
                        position: { xs: 'sticky', sm: 'relative' },
                        bottom: 10,
                        display: { xs: 'flex', sm: 'none' },
                        mt: 2,
                        left: 16,
                        width: { xs: 'calc(100% - 16px)', sm: 'auto' },
                    }}
                /> */}
            </Grid>
        </Stack>
    )
}
{
    /* <KorpaZakljucivanje
                favoriteStoreId={cart.favoriteStoreId}
                paymentTypeId={cart.paymentTypeId}
                oneTimeHash={cartId}
                onProcessStart={() => {
                    setContentDisabled(true)
                    }}
                    onProcessEnd={() => {}}
                        onFail={() => {
                            setContentDisabled(false)
                            }}
                            onSuccess={() => {
                                router.push(`/porudzbine/${cartId}`)
                                }}
                                /> */
}
{
    /* </Box> */
}

export default Korpa
