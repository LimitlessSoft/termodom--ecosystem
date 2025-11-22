import {
    HorizontalActionBar,
    HorizontalActionBarButton,
    AzuriranjeCena,
} from '@/widgets'
import { CircularProgress, Grid } from '@mui/material'
import { useUser } from '@/hooks/useUserHook'
import { useState } from 'react'

const WebProdavnica = () => {
    const user = useUser()
    const [content, setContent] = useState('CENE')

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
                        text="Ažuriranje Web Cena"
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
