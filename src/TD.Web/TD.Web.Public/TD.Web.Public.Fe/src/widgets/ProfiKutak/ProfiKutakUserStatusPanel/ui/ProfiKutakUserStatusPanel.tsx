import { useUser } from "@/app/hooks"
import { ProfiKutakPanelBase } from "../../ProfiKutakSkorasnjePorudzbinePanelBase"
import { Button, Divider, Grid, Stack, Typography } from "@mui/material"
import { formatNumber } from "@/app/helpers/numberHelpers"
import { mainTheme } from "@/app/theme"
import { toast } from "react-toastify"

export const ProfiKutakUserStatusPanel = (): JSX.Element => {

    const user = useUser(false, false)

    return (
        <ProfiKutakPanelBase title={`Informacije korisnika`}>
            <Stack
                m={1}
                spacing={1}
                direction={`column`}>
                <Typography>
                    Korisnik: {user?.data?.nickname}
                </Typography>
                <Typography>
                    Ukupan broj porudžbina: 0
                </Typography>
                <Typography
                    color={mainTheme.palette.success.main}>
                    Ukupna ušteda: {formatNumber(0)}
                </Typography>

                <Divider sx={{ py: 1 }} />

                <Button
                    variant={`contained`}
                    onClick={() => {
                        toast.error(`Ova funkcionalnost još uvek nije implementirana`)
                    }}>
                    Resetuj lozinku
                </Button>
            </Stack>
        </ProfiKutakPanelBase>
    )
}