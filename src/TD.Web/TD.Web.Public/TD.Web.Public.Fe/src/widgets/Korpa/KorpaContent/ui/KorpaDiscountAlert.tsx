import { ApiBase, fetchApi } from "@/app/api"
import { CookieNames } from "@/app/constants"
import { formatNumber } from "@/app/helpers/numberHelpers"
import { Grid, LinearProgress, Typography } from "@mui/material"
import { useEffect, useState } from "react"
import useCookie from 'react-use-cookie'

export const KorpaDiscountAlert = (props: any): JSX.Element => {

    const [cartId, setCartId] = useCookie(CookieNames.cartId)
    const [currentCartLevel, setCurrentCartLevel] = useState<any | null>(null)

    useEffect(() => {
        fetchApi(ApiBase.Main, `/cart-current-level-information?oneTimeHash=${cartId}`)
        .then((res) => {
            setCurrentCartLevel(res)
        })
    }, [])

    return (
        props.cart == null || currentCartLevel == null ?
        <LinearProgress /> :
        <Grid m={5}>
            <Typography
                align={`justify`}>
                Trenutna ukupna vrednost vašeg računa bez PDV-a iznosi {formatNumber(props.cart.summary.valueWithoutVAT)} i dodeljeni su Vam rabati stepena {currentCartLevel.currentLevel}.
                Ukoliko ukupna vrednost računa pređe {formatNumber(currentCartLevel.nextLevelValue)} RSD stepen rabata će biti ažuriran!
                *Rabat se obracunava na ukupnu vrednost korpe bez pdv-a
            </Typography>
        </Grid>
    )
}