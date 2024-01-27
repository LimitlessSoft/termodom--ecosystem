import { ApiBase, fetchApi } from "@/app/api"
import { CookieNames, UIDimensions } from "@/app/constants"
import { KorpaContent } from "@/widgets/Korpa/KorpaContent"
import { KorpaDiscountAlert } from "@/widgets/Korpa/KorpaContent/ui/KorpaDiscountAlert"
import { KorpaZakljucivanje } from "@/widgets/Korpa/KorpaContent/ui/KorpaZakljucivanje"
import { KorpaEmpty } from "@/widgets/Korpa/KorpaEmpty"
import { HorizontalActionBar, HorizontalActionBarButton } from "@/widgets/TopActionBar"
import { Grid, LinearProgress, Typography } from "@mui/material"
import { useRouter } from "next/router"
import { useEffect, useState } from "react"
import useCookie from 'react-use-cookie'

const Korpa = (): JSX.Element => {

    const [cartId, setCartId] = useCookie(CookieNames.cartId)
    const [cart, setCart] = useState<any>(null)
    const router = useRouter()

    const ucitajKorpu = (cartId: string) => {
        fetchApi(ApiBase.Main, `/cart?oneTimeHash=${cartId}`)
        .then((res) => {
            setCart(res)
        })
    }

    useEffect(() => {
        ucitajKorpu(cartId)
    }, [cartId])

    return (
        cart == null ?
            <LinearProgress /> :
                cart.items.length == 0 ?
                    <KorpaEmpty /> :
                    <Grid
                        maxWidth={UIDimensions.maxWidth}
                        margin={`auto`}>
                        <HorizontalActionBar>
                            <HorizontalActionBarButton text={`Nastavi kupovinu`} onClick={() => {
                                router.push(`/`)
                            }} />
                        </HorizontalActionBar>
                        <KorpaContent cart={cart} />
                        <KorpaDiscountAlert />
                        <KorpaZakljucivanje />
                    </Grid>
    )
}

export default Korpa