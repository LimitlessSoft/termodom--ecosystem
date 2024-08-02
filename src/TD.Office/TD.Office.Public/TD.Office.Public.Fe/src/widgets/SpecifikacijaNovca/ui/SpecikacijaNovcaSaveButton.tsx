import { mainTheme } from '@/themes'
import { Button, Grid } from '@mui/material'

export const SpecifikacijaNovcaSaveButton = (props: any) => {
    return (
        <Grid item sm={12} textAlign={`right`}>
            <Button
                onClick={props.onClick}
                variant={`contained`}
                size={`large`}
                sx={{
                    fontSize: mainTheme.typography.h5.fontSize,
                }}
            >
                Sacuvaj specifikaciju
            </Button>
        </Grid>
    )
}
