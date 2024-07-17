import {
    HorizontalActionBar,
    HorizontalActionBarButton,
    AzuriranjeCena,
} from '@/widgets'
import { CircularProgress, Grid } from '@mui/material'
import { useUser } from '@/hooks/useUserHook'
import { useState } from 'react'

enum WebProdavnicaContent {
    CENE,
}

const WebProdavnica = () => {
    const user = useUser()
    const [content, setContent] = useState<WebProdavnicaContent>(
        WebProdavnicaContent.CENE
    )

    const Content = () => {
        switch (content) {
            default:
                return <AzuriranjeCena />
        }
    }

    return user?.isLogged == null || user.isLogged == false ? (
        <CircularProgress />
    ) : (
        <Grid container direction={`column`}>
            <Grid item sm={12}>
                <HorizontalActionBar>
                    <HorizontalActionBarButton
                        text="Ažuriranje cena"
                        onClick={() => {
                            setContent(WebProdavnicaContent.CENE)
                        }}
                    />
                </HorizontalActionBar>
            </Grid>
            <Grid item sm={12}>
                <Content />
            </Grid>
        </Grid>
    )
}

export default WebProdavnica
