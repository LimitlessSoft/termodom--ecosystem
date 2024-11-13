import { Grid, styled } from '@mui/material'

export const KorisniciSingularDataFieldStyled = styled(Grid)(
    ({ theme, editable }) => `
        margin: ${theme.spacing(0.5)} 0;

        input {
            -webkit-text-fill-color: rgba(0, 0, 0, 1) !important;
        }

        fieldset {
            border-color: ${editable ? `black` : theme.palette.grey[300]} !important;
        }
    `
)
