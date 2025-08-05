import { formatNumber } from '@/app/helpers/numberHelpers'
import { ResponsiveTypography } from '@/widgets/Responsive'
import { Grid, LinearProgress } from '@mui/material'
import { useEffect, useState } from 'react'
import { handleApiError, webApi } from '@/api/webApi'

export const KorpaDiscountAlert = (props) => {
    const [currentCartLevel, setCurrentCartLevel] = useState(null)

    useEffect(() => {
        const loadCartCurrentLevel = () => {
            webApi
                .get(
                    `/cart-current-level-information?oneTimeHash=${props.cartId}`
                )
                .then((res) => setCurrentCartLevel(res.data))
                .catch((err) => handleApiError(err))
        }

        loadCartCurrentLevel()

        const interval = setInterval(() => {
            loadCartCurrentLevel()
        }, props.reloadInterval)

        return () => clearInterval(interval)
    }, [props.cartId, props.reloadInterval, props.valueWithoutVAT])

    return !props.valueWithoutVAT || !currentCartLevel ? (
        <LinearProgress />
    ) : (
        <Grid
            item
            my={5}
            mx={2}
            sx={{
                height: 'max-content',
                gridColumn: { xs: '1', md: '2' },
                gridRow: { xs: 'auto', md: '1' },
            }}
        >
            {!currentCartLevel.nextLevelValue ? (
                <ResponsiveTypography align={`center`}>
                    Vaša korpa je trenutno na najvišem nivou rabata!
                </ResponsiveTypography>
            ) : (
                <ResponsiveTypography align={`justify`}>
                    Trenutna ukupna vrednost vašeg računa bez PDV-a iznosi{' '}
                    {formatNumber(props.valueWithoutVAT)} i dodeljeni su Vam
                    rabati stepena {currentCartLevel.currentLevel}. Ukoliko
                    ukupna vrednost računa pređe{' '}
                    {formatNumber(currentCartLevel.nextLevelValue)} RSD stepen
                    rabata će biti ažuriran! *Rabat se obracunava na ukupnu
                    vrednost korpe bez pdv-a
                </ResponsiveTypography>
            )}
        </Grid>
    )
}
