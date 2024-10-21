import { Button, Grid } from '@mui/material'
import { useRouter } from 'next/router'

export default function Layout({ children }: any) {
    const router = useRouter()

    return (
        <Grid container gap={2}>
            <Grid item xs={12}>
                <Button variant={`contained`} onClick={() => router.push('as')}>
                    Finansijsko i Komercijalno
                </Button>
            </Grid>
            {children}
        </Grid>
    )
}
