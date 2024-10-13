import { USER_PERMISSIONS } from '@/constants'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { mainTheme } from '@/themes'
import { Button, Grid } from '@mui/material'

export const SpecifikacijaNovcaSaveButton = ({ onClick, permissions }: any) => {
    return (
        <Grid item sm={12} textAlign={`right`}>
            <Button
                onClick={onClick}
                variant={`contained`}
                size={`large`}
                sx={{
                    fontSize: mainTheme.typography.h5.fontSize,
                }}
                disabled={
                    !hasPermission(
                        permissions,
                        USER_PERMISSIONS.SPECIFIKACIJA_NOVCA.SAVE
                    )
                }
            >
                Sacuvaj specifikaciju
            </Button>
        </Grid>
    )
}
