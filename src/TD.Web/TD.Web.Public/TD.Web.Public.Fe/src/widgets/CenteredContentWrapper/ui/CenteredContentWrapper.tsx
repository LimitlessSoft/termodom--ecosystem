import { Grid } from '@mui/material'
import { ReactNode } from 'react'

interface ICenteredContentWrapperProps {
    children: ReactNode
}

export const CenteredContentWrapper = (
    props: ICenteredContentWrapperProps
): JSX.Element => {
    const { children } = props
    return (
        <Grid
            container
            sx={{
                maxWidth: '1100px',
                minHeight: '100vh',
                margin: 'auto',
                my: 2,
                px: {
                    xs: 2,
                    lg: 0,
                },
            }}
        >
            {children}
        </Grid>
    )
}
