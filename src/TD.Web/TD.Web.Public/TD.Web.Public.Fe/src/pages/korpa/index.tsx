import {
    HorizontalActionBar,
    HorizontalActionBarButton,
} from '@/widgets/TopActionBar'
import { KorpaDiscountAlert } from '@/widgets/Korpa/KorpaContent/ui/KorpaDiscountAlert'
import { KorpaZakljucivanje } from '@/widgets/Korpa/KorpaContent/ui/KorpaZakljucivanje'
import { KorpaSummary } from '@/widgets/Korpa/KorpaContent/ui/KorpaSummary'
import { CookieNames, KorpaTitle, UIDimensions } from '@/app/constants'
import { KorpaContent } from '@/widgets/Korpa/KorpaContent'
import { KorpaEmpty } from '@/widgets/Korpa/KorpaEmpty'
import { Grid, LinearProgress } from '@mui/material'
import { CustomHead } from '@/widgets/CustomHead'
import { useEffect, useState } from 'react'
import useCookie from 'react-use-cookie'
import { useRouter } from 'next/router'
import { useUser } from '@/app/hooks'
import { webApi } from '@/api/webApi'

const Korpa = (): JSX.Element => {
    const user = useUser(false, true)
    const [cartId, setCartId] = useCookie(CookieNames.cartId)
    const [cart, setCart] = useState<any>(null)
    const router = useRouter()
    const [contentDisabled, setContentDisabled] = useState<boolean>(false)

    const ucitajKorpu = (cartId: string | null, isLogged: boolean) => {
        webApi
            .get(`/cart?oneTimeHash=${cartId}`)
            .then((res) => setCart(res.data))
    }

    const reloadInterval = 5000

    useEffect(() => {
        if (user == null || user.isLoading) return

        const ucitavanjeKorpe = () => ucitajKorpu(cartId, user.isLogged)

        ucitavanjeKorpe()

        const interval = setInterval(() => {
            ucitavanjeKorpe()
        }, reloadInterval)

        return () => clearInterval(interval)
    }, [user, cartId])

    return !cart ? (
        <LinearProgress />
    ) : cart.items.length == 0 ? (
        <KorpaEmpty />
    ) : (
        <Grid maxWidth={UIDimensions.maxWidth} margin={`auto`}>
            <CustomHead title={KorpaTitle} />
            <HorizontalActionBar>
                <HorizontalActionBarButton
                    text={`Nastavi kupovinu`}
                    onClick={() => {
                        router.push(`/`)
                    }}
                />
            </HorizontalActionBar>
            <KorpaContent
                elementsDisabled={contentDisabled}
                cart={cart}
                reloadKorpa={() => {
                    ucitajKorpu(cartId, user.isLogged)
                }}
                onItemRemove={(it) => {
                    setCart((prev: any) => {
                        return {
                            ...prev,
                            items: prev.items.filter((i: any) => i.id != it.id),
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
