import { Grid, styled } from '@mui/material'

export const KorisnikAnalizaPanelStyled = styled(Grid)(
    ({ theme }) => `
        padding: ${theme.spacing(2)};
        border: 1px solid ${theme.palette.divider};
        border-radius: ${theme.shape.borderRadius};
        background-color: ${theme.palette.background.paper};
    `
)
