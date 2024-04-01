import { HorizontalActionBar, HorizontalActionBarButton } from "@/widgets/TopActionBar"
import { KorpaDiscountAlert } from "@/widgets/Korpa/KorpaContent/ui/KorpaDiscountAlert"
import { KorpaZakljucivanje } from "@/widgets/Korpa/KorpaContent/ui/KorpaZakljucivanje"
import { CookieNames, KorpaTitle, UIDimensions } from "@/app/constants"
import { KorpaSummary } from "@/widgets/Korpa/KorpaContent/ui/KorpaSummary"
import { Grid, LinearProgress, Typography } from "@mui/material"
import { KorpaContent } from "@/widgets/Korpa/KorpaContent"
import { KorpaEmpty } from "@/widgets/Korpa/KorpaEmpty"
import { ApiBase, fetchApi } from "@/app/api"
import { useEffect, useState } from "react"
import useCookie from 'react-use-cookie'
import { useRouter } from "next/router"
import { useUser } from "@/app/hooks"
import { CustomHead } from "@/widgets/CustomHead"
 
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
                        <CustomHead title={KorpaTitle} />
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
                        <KorpaSummary cart={cart} />
                        <KorpaZakljucivanje
                            favoriteStoreId={cart.favoriteStoreId}
                            oneTimeHash={cartId}
                            onProcessStart={() => {
                                setContentDisabled(true)
                            }}
                            onProcessEnd={() => {
                                
                            }}
                            onFail={() => {
                                setContentDisabled(false)
                            }}
                            onSuccess={() => {
                                console.log(`Success: ` + cartId)
                                router.push(`/porudzbine/${cartId}`)
                            }} />
                    </Grid>
    )
}

export default Korpa