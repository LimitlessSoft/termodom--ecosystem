import { ApiBase, fetchApi } from '@/app/api'
import { CookieNames } from '@/app/constants'
import { formatNumber } from '@/app/helpers/numberHelpers'
import { ResponsiveTypography } from '@/widgets/Responsive'
import { Grid, LinearProgress, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import useCookie from 'react-use-cookie'
import { IKorpaDiscountAlertProps } from '../interfaces/IKorpaDiscountAlertProps'

export const KorpaDiscountAlert = (
    props: IKorpaDiscountAlertProps
): JSX.Element => {
    const [cartId, setCartId] = useCookie(CookieNames.cartId)
    const [currentCartLevel, setCurrentCartLevel] = useState<any | null>(null)

    const loadCartCurrentLevel = () => {
        fetchApi(
            ApiBase.Main,
            `/cart-current-level-information?oneTimeHash=${cartId}`
        ).then((res: any) => {
            res.json().then((res: any) => {
                setCurrentCartLevel(res)
            })
        })
    }

    var currentLevelInterval: any = null

    useEffect(() => {
        loadCartCurrentLevel()

        if (!currentLevelInterval) {
            clearInterval(currentLevelInterval)
        }

        currentLevelInterval = setInterval(() => {
            loadCartCurrentLevel()
        }, props.reloadInterval)
    }, [props.cart])

    return props.cart == null || currentCartLevel == null ? (
        <LinearProgress />
    ) : (
        <Grid m={5}>
            {currentCartLevel.nextLevelValue == null ? (
                <ResponsiveTypography align={`center`}>
                    Vaša korpa je trenutno na najvišem nivou rabata!
                </ResponsiveTypography>
            ) : (
                <ResponsiveTypography align={`justify`}>
                    Trenutna ukupna vrednost vašeg računa bez PDV-a iznosi{' '}
                    {formatNumber(props.cart.summary.valueWithoutVAT)} i
                    dodeljeni su Vam rabati stepena{' '}
                    {currentCartLevel.currentLevel}. Ukoliko ukupna vrednost
                    računa pređe {formatNumber(currentCartLevel.nextLevelValue)}{' '}
                    RSD stepen rabata će biti ažuriran! *Rabat se obracunava na
                    ukupnu vrednost korpe bez pdv-a
                </ResponsiveTypography>
            )}
        </Grid>
    )
}
