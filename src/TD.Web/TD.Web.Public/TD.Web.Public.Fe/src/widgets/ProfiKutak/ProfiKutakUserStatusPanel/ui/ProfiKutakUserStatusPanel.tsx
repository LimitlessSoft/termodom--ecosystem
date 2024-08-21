import { ProfiKutakPanelBase } from '../../ProfiKutakSkorasnjePorudzbinePanelBase'
import { CircularProgress, Divider, Stack } from '@mui/material'
import { formatNumber } from '@/app/helpers/numberHelpers'
import { mainTheme } from '@/app/theme'
import { useEffect, useState } from 'react'
import { ResponsiveTypography } from '@/widgets/Responsive'
import { PostaviNovuLozinku } from './PostavniNovuLozinku'
import { handleApiError, webApi } from '@/api/webApi'

export const ProfiKutakUserStatusPanel = (): JSX.Element => {
    const [userInfo, setUserInfo] = useState<any | null>(null)

    useEffect(() => {
        webApi
            .get('/orders-info')
            .then((res) => {
                setUserInfo(res.data)
            })
            .catch((err) => handleApiError(err))
    }, [])

    return (
        <ProfiKutakPanelBase title={`Informacije korisnika`}>
            {!userInfo ? (
                <CircularProgress />
            ) : (
                <Stack m={1} spacing={1} direction={`column`}>
                    <ResponsiveTypography>
                        Korisnik: {userInfo.user}
                    </ResponsiveTypography>
                    <ResponsiveTypography>
                        Ukupan broj porudžbina: {userInfo.numberOfOrders}
                    </ResponsiveTypography>
                    <ResponsiveTypography
                        color={mainTheme.palette.success.main}
                        fontFamily={`GothamProMedium`}
                    >
                        Ukupna ušteda:{' '}
                        {formatNumber(userInfo.totalDiscountValue)}
                    </ResponsiveTypography>

                    <Divider sx={{ py: 1 }} />

                    <PostaviNovuLozinku />
                </Stack>
            )}
        </ProfiKutakPanelBase>
    )
}
