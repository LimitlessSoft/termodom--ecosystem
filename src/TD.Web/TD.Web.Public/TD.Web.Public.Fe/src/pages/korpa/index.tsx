import { ApiBase, fetchApi } from "@/app/api"
import { CookieNames, UIDimensions } from "@/app/constants"
import { useUser } from "@/app/hooks"
import { KorpaContent } from "@/widgets/Korpa/KorpaContent"
import { KorpaDiscountAlert } from "@/widgets/Korpa/KorpaContent/ui/KorpaDiscountAlert"
import { KorpaZakljucivanje } from "@/widgets/Korpa/KorpaContent/ui/KorpaZakljucivanje"
import { KorpaEmpty } from "@/widgets/Korpa/KorpaEmpty"
import { HorizontalActionBar, HorizontalActionBarButton } from "@/widgets/TopActionBar"
import { Grid, LinearProgress, Typography } from "@mui/material"
import { useRouter } from "next/router"
import { useCallback, useEffect, useState } from "react"
import useCookie from 'react-use-cookie'

const Korpa = (): JSX.Element => {

    const user = useUser(false, true)
    const [cartId, setCartId] = useCookie(CookieNames.cartId)
    const [cart, setCart] = useState<any>(null)
    const router = useRouter()
    const [contentDisabled, setContentDisabled] = useState<boolean>(false)

    const ucitajKorpu = (cartId: string | null, isLogged: boolean) => {
        let route = `/cart`

        if(!isLogged)
            route += `?oneTimeHash=${cartId}`

        fetchApi(ApiBase.Main, route)
            .then((res) => {
                setCart(res)
            })
    }

    useEffect(() => {
        if(user == null || user.isLoading)
            return

        ucitajKorpu(cartId, user.isLogged)
    }, [user, cartId])

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
                        <KorpaContent
                            elementsDisabled={contentDisabled}
                            cart={cart}
                            reloadKorpa={() => {
                                ucitajKorpu(cartId, user.isLogged)
                            }}
                            onItemRemove={(it) => {
                                setCart((prev: any) => {
                                    return { ...prev, items: prev.items.filter((i: any) => i.id != it.id) }
                                })}
                        }/>
                        {
                            user.isLogged == false && cart != null ? <KorpaDiscountAlert cart={cart} /> : null
                        }
                        <KorpaZakljucivanje
                            oneTimeHash={cartId}
                            onProcessStart={() => {
                                setContentDisabled(true)
                            }}
                            onProcessEnd={() => {
                                
                            }}
                            onSuccess={() => {
                                ucitajKorpu(null, user.isLogged)
                            }} />
                    </Grid>
    )
}

export default Korpa