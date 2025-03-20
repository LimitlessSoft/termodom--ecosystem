import { KorpaDiscountAlert } from '@/widgets/Korpa/KorpaContent/ui/KorpaDiscountAlert'
import { KorpaZakljucivanje } from '@/widgets/Korpa/KorpaContent/ui/KorpaZakljucivanje'
import { KorpaSummary } from '@/widgets/Korpa/KorpaContent/ui/KorpaSummary'
import { CookieNames, KorpaTitle, UIDimensions } from '@/app/constants'
import { KorpaContent } from '@/widgets/Korpa/KorpaContent'
import { KorpaEmpty } from '@/widgets/Korpa/KorpaEmpty'
import {
    Alert,
    Box,
    Button,
    Grid,
    LinearProgress,
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
import { ArrowDownward } from '@mui/icons-material'

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
        <Grid
            maxWidth={UIDimensions.maxWidth}
            margin={`auto`}
            sx={{ position: `relative`, mt: 2 }}
        >
            <CustomHead title={KorpaTitle} />
            <HorizontalActionBar
                spacing={2}
                backButton={{
                    title: 'Nastavi kupovinu',
                    href: '/',
                }}
            >
                <HorizontalActionBarButton
                    href={`#orderForm`}
                    variant={`contained`}
                    color="success"
                    endIcon={<ArrowDownward />}
                    text={`Završi porudžbinu`}
                />
            </HorizontalActionBar>
            <Box container p={2}>
                <Alert severity="info" variant={`filled`}>
                    Povećajte ukupnu vrednost korpe za veći popust!
                </Alert>
            </Box>
            <KorpaContent
                elementsDisabled={contentDisabled}
                cart={cart}
                reloadKorpa={() => {
                    ucitajKorpu(cartId)
                }}
                onItemRemove={(it) => {
                    setCart((prev) => {
                        return {
                            ...prev,
                            items: prev.items.filter((i) => i.id !== it.id),
                        }
                    })
                }}
            />
            {!user.isLogged && cart && (
                <KorpaDiscountAlert
                    cartId={cartId}
                    valueWithoutVAT={cart.summary.valueWithoutVAT}
                    reloadInterval={reloadInterval}
                />
            )}
            <KorpaSummary cart={cart} />
            <KorpaZakljucivanje
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
            />
        </Grid>
    )
}

export default Korpa
