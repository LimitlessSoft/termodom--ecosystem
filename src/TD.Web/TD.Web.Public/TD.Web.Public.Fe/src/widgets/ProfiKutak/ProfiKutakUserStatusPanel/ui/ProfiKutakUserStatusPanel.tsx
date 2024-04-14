import { useUser } from "@/app/hooks"
import { ProfiKutakPanelBase } from "../../ProfiKutakSkorasnjePorudzbinePanelBase"
import { Button, CircularProgress, Divider, Grid, Stack, Typography } from "@mui/material"
import { formatNumber } from "@/app/helpers/numberHelpers"
import { mainTheme } from "@/app/theme"
import { toast } from "react-toastify"
import { useEffect, useState } from "react"
import { ApiBase, fetchApi } from "@/app/api"
import { ResponsiveTypography } from "@/widgets/Responsive"
import { PostaviNovuLozinku } from "./PostavniNovuLozinku"

export const ProfiKutakUserStatusPanel = (): JSX.Element => {

    const [userInfo, setUserInfo] = useState<any | null>(null)

    useEffect(() => {
        fetchApi(ApiBase.Main, `/orders-info`)
        .then((res) => {
            setUserInfo(res)
        })
    }, [])

    return (
        <ProfiKutakPanelBase title={`Informacije korisnika`}>
        {
            userInfo == null ?
            <CircularProgress /> :
            <Stack
                m={1}
                spacing={1}
                direction={`column`}>
                <ResponsiveTypography>
                    Korisnik: {userInfo.user}
                </ResponsiveTypography>
                <ResponsiveTypography>
                    Ukupan broj porudžbina: {userInfo.numberOfOrders}
                </ResponsiveTypography>
                <ResponsiveTypography
                    color={mainTheme.palette.success.main}
                    fontFamily={`GothamProMedium`}>
                    Ukupna ušteda: {formatNumber(userInfo.totalDiscountValue)}
                </ResponsiveTypography>

                <Divider sx={{ py: 1 }} />

                <PostaviNovuLozinku />
            </Stack>
        }
        </ProfiKutakPanelBase>
    )
}